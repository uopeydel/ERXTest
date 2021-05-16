using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ERXTest.Shared.Helpers
{
    public static class ERXTestResponse
    {

        // Usage:
        // string queryString = GetQueryString(foo);
        public static string GetQueryString(object obj)
        {
            if (obj == null || obj == default)
            {
                return string.Empty;
            }
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return "?" + String.Join("&", properties.ToArray());
        }

        public static ERXTestResults<TResult> CreateSuccessResponse<TResult>(TResult tresult,
            PagingInfo pagingInfo = null, List<string> message = null)
        {
            var result = new ERXTestResults<TResult>();
            result.Data = tresult;
            result.PageInfo = pagingInfo;
            //result.StatusCode = 200;
            result.Error = false;
            result.Message = message ?? new List<string>();
            return result;
        }

        public static ERXTestResults<TResult> CreateErrorResponse<TResult>(string error, Exception exception = null)
        {
            var result = new ERXTestResults<TResult>();
            if (result.Message == null)
            {
                result.Message = new List<string>
                {
                    exception?.Message
                };
            }

            result.Message.AddRange(GetInnerExceptionMessage(exception));
            result.Message.Insert(0, error);
            result.Message = result?.Message?.Where(w => !string.IsNullOrEmpty(w)).Distinct().ToList();
            //result.StatusCode = 500;
            result.Error = true;
            return result;
        }
        public static ERXTestResults<TResult> CreateErrorResponse<TResult>(List<string> errors, Exception exception = null)
        {
            return CreateErrorResponse<TResult>(errors.ToArray(), exception);
        }
        public static ERXTestResults<TResult> CreateErrorResponse<TResult>(string[] errors, Exception exception = null)
        {
            var result = new ERXTestResults<TResult>();
            result.Message = GetInnerExceptionMessage(exception);
            foreach (var error in errors)
            {
                result.Message.Insert(0, error);
            }

            result.Message = result?.Message?.Where(w => !string.IsNullOrEmpty(w)).Distinct().ToList();
            //result.StatusCode = 500;
            result.Error = true;
            return result;
        }

        private static List<string> GetInnerExceptionMessage(Exception exception)
        {
            var errorTextList = new List<string>();
            var error = GetSubInnerExceptionMessage(exception);
            while (!string.IsNullOrEmpty(error?.Trim()))
            {
                errorTextList.Add(error);
                if (errorTextList.Count() > 10)
                {
                    errorTextList = errorTextList.Distinct().ToList();
                    break;
                }

                error = GetSubInnerExceptionMessage(exception);
            }

            return errorTextList;
        }

        private static string GetSubInnerExceptionMessage(Exception exception)
        {
            return exception?.InnerException?.Message;
        }


        public static Expression<Func<T, bool>> AndAlso<T>(
            Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }

        private class ReplaceExpressionVisitor
            : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }
    }
}
