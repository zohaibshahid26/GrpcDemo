using gRPC_for_ASPNET_CORE.Model;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using GrpcDemo;
using gRPC_for_ASPNET_CORE.Data;

namespace gRPC_for_ASPNET_CORE.Services
{
    public class ProductsService : Products.ProductsBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<AddProductResponse> AddProduct(AddProductRequest request, ServerCallContext context)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Name) || request.Price <= 0 || request.Qty <= 0)
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid data"));

                var product = new Product
                {
                    Name = request.Name,
                    Price = request.Price,
                    Qty = request.Qty
                };
                _dbContext.Products.Add(product);
                await _dbContext.SaveChangesAsync();

                return new AddProductResponse
                {
                    Id = product.Id.ToString()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Id))
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid data"));

                var product = await _dbContext.Products.FindAsync(Guid.TryParse(request.Id, out var id) ? id : Guid.Empty);
                if (product == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
                }

                return new GetProductResponse
                {
                    Id = product.Id.ToString(),
                    Name = product.Name,
                    Price = product.Price,
                    Qty = product.Qty
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public override async Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Id) || string.IsNullOrEmpty(request.Name) || request.Price <= 0 || request.Qty <= 0)
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid data"));

                var product = await _dbContext.Products.FindAsync(Guid.TryParse(request.Id, out var id) ? id : Guid.Empty);
                if (product == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
                }

                product.Name = request.Name;
                product.Price = request.Price;
                product.Qty = request.Qty;

                _dbContext.Products.Update(product);
                await _dbContext.SaveChangesAsync();

                return new UpdateProductResponse
                {
                    Id = product.Id.ToString()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public override async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Id))
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid data"));

                var product = await _dbContext.Products.FindAsync(Guid.TryParse(request.Id, out var id) ? id : Guid.Empty);
                if (product == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
                }

                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();

                return new DeleteProductResponse
                {
                    Id = request.Id
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public override async Task<GetAllProductResponse> GetAllProduct(GetAllProductRequest request, ServerCallContext context)
        {
            try
            {
                var products = await _dbContext.Products.ToListAsync();

                var response = new GetAllProductResponse();
                foreach (var product in products)
                {
                    response.Products.Add(new GetProductResponse
                    {
                        Id = product.Id.ToString(),
                        Name = product.Name,
                        Price = product.Price,
                        Qty = product.Qty
                    });
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }
    }
}
