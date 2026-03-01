using Discount.Entities;
using Discount.Features.Discount.DTOs;
using Discount.Features.Shared;
using FluentValidation;

namespace Discount.Features.Discount
{
    public class DiscountService
    {
        private readonly DapperContext _dbContext;
        private readonly IValidator<CreateDiscountRequest> _createValidator;
        private readonly IValidator<UpdateDiscountRequest> _updateValidator;

        public DiscountService(DapperContext dbContext, 
                               IValidator<CreateDiscountRequest> createValidator,
                               IValidator<UpdateDiscountRequest> updateValidator)
        {
            _dbContext = dbContext;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        // create discount
        public async Task<ServiceResult> CreateDiscount(CreateDiscountRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);

            if (validationResult.IsValid)
            {
                return new ServiceResult
                {
                    IsSuccessful = false,
                    ErrorType = ErrorType.GrpcValidationError,
                    ErrorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
                };
            }

            var result = await _dbContext.ExecuteAsync(
                "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { request.ProductName, request.Description, request.Amount });
            if (result > 0)
            {
                return new ServiceResult
                {
                    IsSuccessful = true,
                };
            }
            return new ServiceResult
            {
                IsSuccessful = false,
                ErrorType = ErrorType.GrpcError
            };
        }

        // get discount by product name
        public async Task<ServiceResult<Coupon>> GetDiscount(string productName)
        {
            // validate product name is not empty
            if (string.IsNullOrEmpty(productName))
            {
                return new ServiceResult<Coupon>
                {
                    IsSuccessful = false,
                    ErrorType = ErrorType.GrpcValidationError,
                    ErrorMessage = "Product name is required"
                };
            }

            var result = await _dbContext.QueryFirstOrDefaultAsync<Coupon>(
                "SELECT * FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });
            if (result != null)
            {
                return new ServiceResult<Coupon>
                {
                    Data = result,
                    IsSuccessful = true
                };
            }
            return new ServiceResult<Coupon>
            {
                IsSuccessful = false,
                ErrorType = ErrorType.GrpcError,
                ErrorMessage = $"Discount for product {productName} not found"
            };
        }


        // delete discount by product name
        public async Task<ServiceResult> DeleteDiscount(string productName)
        {
            // validate product name is not empty
            if (string.IsNullOrEmpty(productName))
            {
                return new ServiceResult
                {
                    IsSuccessful = false,
                    ErrorType = ErrorType.GrpcValidationError,
                    ErrorMessage = "Product name is required"
                };
            }

            var result = await _dbContext.ExecuteAsync(
                "DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });
            if (result > 0)
            {
                return new ServiceResult
                {
                    IsSuccessful = true,
                };
            }
            return new ServiceResult
            {
                IsSuccessful = false,
                ErrorType = ErrorType.GrpcError,
                ErrorMessage = $"Discount for product {productName} not found"
            };
        }

        // update discount
        public async Task<ServiceResult> UpdateDiscount(UpdateDiscountRequest request)
        {

            var validationResult = await _updateValidator.ValidateAsync(request);
            if (validationResult != null) {
                return new ServiceResult
                {
                    IsSuccessful = false,
                    ErrorType = ErrorType.GrpcValidationError,
                    ErrorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
                };
            }

            var result = await _dbContext.ExecuteAsync(
                "UPDATE Coupon SET Description = @Description, Amount = @Amount WHERE ProductName = @ProductName",
                new { request.Description, request.Amount, request.ProductName });
            if (result > 0)
            {
                return new ServiceResult
                {
                    IsSuccessful = true,
                };
            }
            return new ServiceResult
            {
                IsSuccessful = false,
                ErrorType = ErrorType.GrpcError,
                ErrorMessage = $"Discount for product {request.ProductName} not found"
            };
        }
    }
}
