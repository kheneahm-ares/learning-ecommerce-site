using Discount.Extensions;
using Grpc.Core;

namespace Discount.Features.Shared
{
    public static class ServiceResultExtensions
    {
        public static RpcException ToGrpcException(this ServiceResult serviceResult)
        {
            if (serviceResult.ErrorType == ErrorType.GrpcValidationError)
            {
                return GrpcErrorExtensions.CreateValidationException(
                    new Dictionary<string, string> { { "error", serviceResult.ErrorMessage } });
            }

            return new RpcException(new Status(StatusCode.Internal, serviceResult.ErrorMessage ?? "An error occurred"));
        }

        public static RpcException ToGrpcException<T>(this ServiceResult<T> serviceResult)
        {
            if (serviceResult.ErrorType == ErrorType.GrpcValidationError)
            {
                return GrpcErrorExtensions.CreateValidationException(
                    new Dictionary<string, string> { { "error", serviceResult.ErrorMessage } });
            }

            return new RpcException(new Status(StatusCode.NotFound, serviceResult.ErrorMessage ?? "Not found"));
        }
    }
}
