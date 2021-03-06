﻿using System;
using System.Linq.Expressions;
using System.Reflection;


namespace Shiny
{
    public static class Extensions_Reflection
    {
        public static PropertyInfo GetPropertyInfo<TSender, TRet>(this TSender sender, Expression<Func<TSender, TRet>> expression)
        {
            if (sender == null)
                throw new ArgumentException("Sender is null");

            var member = (expression as LambdaExpression)?.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException("Invalid lamba expression - body is not a member expression");

            var property = sender.GetType().GetRuntimeProperty(member.Member.Name);
            return property;
        }


        public static Type Unwrap(this Type type)
        {
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = Nullable.GetUnderlyingType(type);

            return type;
        }
    }
}
