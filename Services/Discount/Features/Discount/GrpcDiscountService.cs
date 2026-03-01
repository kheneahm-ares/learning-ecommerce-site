using Discount.Features.Shared;
using Discount.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using DtoNs = Discount.Features.Discount.DTOs;

namespace Discount.Features.Discount
{
    public class GrpcDiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly DiscountService _discountService;

        public GrpcDiscountService(DiscountService discountService)
        {
            _discountService = discountService;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var result = await _discountService.GetDiscount(request.ProductName);
            if (!result.IsSuccessful)
                throw result.ToGrpcException();

            return new CouponModel
            {
                Id = result.Data.Id,
                ProductName = result.Data.ProductName,
                Description = result.Data.Description,
                Amount = result.Data.Amount
            };
        }

        public override async Task<Empty> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var dto = new DtoNs.CreateDiscountRequest
            {
                ProductName = request.ProductName,
                Description = request.Description,
                Amount = request.Amount
            };

            var result = await _discountService.CreateDiscount(dto);
            if (!result.IsSuccessful)
                throw result.ToGrpcException();

            return new Empty();
        }

        public override async Task<Empty> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var dto = new DtoNs.UpdateDiscountRequest
            {
                Id = request.Id,
                ProductName = request.ProductName,
                Description = request.Description,
                Amount = request.Amount
            };

            var result = await _discountService.UpdateDiscount(dto);
            if (!result.IsSuccessful)
                throw result.ToGrpcException();

            return new Empty();
        }

        public override async Task<Empty> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var result = await _discountService.DeleteDiscount(request.ProductName);
            if (!result.IsSuccessful)
                throw result.ToGrpcException();

            return new Empty();
        }
    }
}
