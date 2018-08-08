using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;

namespace CogsLite.AwsStore
{
    public abstract class AwsBaseStore<TData, TEntity>
    {
        private readonly IDynamoDBContext _dbContext;
        private readonly Lazy<IMapper> _getMapper;

        protected AwsBaseStore(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _getMapper = new Lazy<IMapper>(GetMapper);
        }

        protected async Task PutItem(TData item)
        {
            var entity = _getMapper.Value.Map<TEntity>(item);
            await _dbContext.SaveAsync(entity);
        }

        protected async Task<IEnumerable<TData>> Scan()
        {
            var items = new List<TData>();
            var scanResults = _dbContext.ScanAsync<TEntity>(Enumerable.Empty<ScanCondition>());
            while(scanResults.IsDone == false)
            {
                var set = await scanResults.GetNextSetAsync();
                items.AddRange(set.Select(x => _getMapper.Value.Map<TData>(x)));
            }

            return items;
        }

        protected async Task<IEnumerable<TData>> Query(object hashKeyValue)
        {
            var items = new List<TData>();
            var queryResults = _dbContext.QueryAsync<TEntity>(hashKeyValue.ToString());
            while(queryResults.IsDone == false)
            {
                var set = await queryResults.GetNextSetAsync();
                items.AddRange(set.Select(x => _getMapper.Value.Map<TData>(x)));
            }

            return items;
        }

        protected async Task<TData> FindById(Guid hashKey, Guid rangeKey)
        {
            var entity = await _dbContext.LoadAsync<TEntity>(hashKey.ToString(), rangeKey.ToString());
            return _getMapper.Value.Map<TData>(entity);
        }

        protected IDynamoDBContext DbContext => _dbContext;

        protected IMapper GetMapper()
        {
            var config = new MapperConfiguration(c => 
            {
                var outboundMap = c.CreateMap<TData, TEntity>();
                CreateOutboundMap(outboundMap);

                var inboundMap = c.CreateMap<TEntity, TData>();
                CreateInboundMap(inboundMap);
            });

            return config.CreateMapper();
        }

        protected abstract void CreateOutboundMap(IMappingExpression<TData, TEntity> mapping);

        protected abstract void CreateInboundMap(IMappingExpression<TEntity, TData> mapping);
        
    }
}