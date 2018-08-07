using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.DynamoDBv2.Model;
using AutoMapper;
using CogsLite.Core;

namespace CogsLite.AwsStore
{
    public static class MapperExtensions
    {        
        public static Member ToMember(this UserType userType)
        {
            return UserTypeMapper.Map<Member>(userType);
        }
        public static IMappingExpression<TIn, TOut> MapMember<TIn, TOut, TMember>(this IMappingExpression<TIn, TOut> mapping, Expression<Func<TOut, TMember>> propExpr, Func<TIn, TMember> expr)
        {
            return mapping.ForMember(propExpr, opts => opts.ResolveUsing(expr));
        }

        public static IMapper UserTypeMapper => _getMapper.Value;

        private readonly static Lazy<IMapper> _getMapper = new Lazy<IMapper>(GetUserTypeMapper);

        private static IMapper GetUserTypeMapper()
        {
            var mapperConfig = new MapperConfiguration(c => 
            {
                c.CreateMap<UserType, Member>()
                    .MapMember(x => x.EmailAddress, y => y.Attributes.Single(a => a.Name == "email").Value)
                    .MapMember(x => x.Id, y => Guid.Parse(y.Attributes.Single(a => a.Name == "sub").Value));
            });

            return mapperConfig.CreateMapper();
        }
    }        
}
