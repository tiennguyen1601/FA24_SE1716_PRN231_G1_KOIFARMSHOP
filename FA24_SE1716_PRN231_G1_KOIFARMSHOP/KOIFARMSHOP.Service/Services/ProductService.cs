using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data;
using KOIFARMSHOP.Data.DTO.ProductDTO;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Service.Services
{
    public interface IProductService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetAll(int? page, int? size);
        Task<IBusinessResult> GetById(int productId);
        Task<Product> GetProductById(int productId);
        Task<IBusinessResult> Save(Product product, List<string>? images);
        Task<IBusinessResult> DeleteById(int productId);

        Task<IBusinessResult> SearchProducts( ProductFilterReqModel? productFilterReqModel, string? searchValue, int? page, int? size);
        Task<IBusinessResult> GetBrandName();
        Task<IBusinessResult> ActivateDeactivate(int productId);
    }
    public class ProductService : IProductService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IProductImageService _productImageService;

        public ProductService(IProductImageService productImageService)
        {
            _unitOfWork ??= new UnitOfWork();
            _productImageService = productImageService;
        }

        public async Task<IBusinessResult> ActivateDeactivate(int productId)
        {
            try
            {
                var currProduct = await _unitOfWork.ProductRepository.GetByIdAsync(productId);

                if (currProduct == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Product());
                }
                else
                {
                    var result = await _unitOfWork.ProductRepository.ActivateDeactivate(currProduct);

                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, currProduct);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, currProduct);
                    }
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> DeleteById(int productId)
        {
            try
            {
                var currProduct = await _unitOfWork.ProductRepository.GetByIdAsync(productId);

                if (currProduct == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Product());
                }
                else
                {
                    var result = await _unitOfWork.ProductRepository.Delete(currProduct);

                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, currProduct);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, currProduct);
                    }
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetAll()
        {
            var products = await _unitOfWork.ProductRepository.GetProducts();

            if (products == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Product>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, products);
            }
        }

        public async Task<IBusinessResult> GetAll(int? page, int? size)
        {
            var products = await _unitOfWork.ProductRepository.GetProducts();

            var totalItemCount = products.Count;

            var pagedItem = products.Skip(((page ?? 1) - 1) * (size ?? 10))
                    .Take(size ?? 10).ToList();

            var result = new Pagination<Product>
            {
                TotalItems = totalItemCount,
                PageSize = size ?? 10,
                CurrentPage = page ?? 1,
                Data = pagedItem
            };

            return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
        }

        public async Task<IBusinessResult> GetBrandName()
        {
            var allProducts = await _unitOfWork.ProductRepository.GetProducts();

            var products = allProducts;

            var brands = products.Select(x => x.Brand).Distinct().ToList();

            return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, brands);
        }

        public async Task<IBusinessResult> GetById(int productId)
        {
            var currProduct = await _unitOfWork.ProductRepository.GetProductById(productId);

            if (currProduct == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Product>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, currProduct);
            }
        }

        public async Task<Product> GetProductById(int productId)
        {
            var currProduct = await _unitOfWork.ProductRepository.GetByIdAsync(productId);

            return currProduct;
        }

        public async Task<IBusinessResult> Save(Product product, List<string> images)
        {
            try
            {
                int result = -1;
                var currProduct = _unitOfWork.ProductRepository.GetById(product.ProductId);

                if (currProduct != null)
                {
                    #region businessR
                    #endregion businessR

                    result = await _unitOfWork.ProductRepository.UpdateAsync(product);

                    await _productImageService.SaveProductImage(currProduct.ProductId, images);

                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, product);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, product);
                    }

                }
                else
                {
                    result = await _unitOfWork.ProductRepository.CreateAsync(product);

                    await _productImageService.SaveProductImage(product.ProductId, images);

                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, product);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, product);
                    }
                }

            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> SearchProducts(ProductFilterReqModel? productFilterReqModel, string? searchValue, int? page, int? size)
        {
            var allProducts = await _unitOfWork.ProductRepository.GetProducts();

            var products = allProducts;

            int totalItemCount;

            if (!string.IsNullOrEmpty(searchValue))
            {
                products = products.Where(x => x.Name.ToLower().Contains(searchValue.Trim().ToLower())).ToList();

                totalItemCount = products.Count;
            }
            else
            {
                totalItemCount = allProducts.Count;
            }

            if (productFilterReqModel != null)
            {
                products = filterFashionItem(products, productFilterReqModel);
            }

            totalItemCount = products.Count;

            var pagedItem = products.Skip(((page ?? 1) - 1) * (size ?? 10))
                    .Take(size ?? 10).ToList();

            var result = new Pagination<Product>
            {
                TotalItems = totalItemCount,
                PageSize = size ?? 10,
                CurrentPage = page ?? 1,
                Data = pagedItem
            };

            return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
        }

        private List<Product> filterFashionItem(List<Product> products, ProductFilterReqModel productFilterReqModel)
        {
            if (productFilterReqModel.Category is not null && productFilterReqModel.Category.Any())
            {
                products = products
                   .Where(x => productFilterReqModel.Category
                   .Contains(x.Category.Name)).ToList();
            }

            if (productFilterReqModel.Brand is not null && productFilterReqModel.Brand.Any())
            {
                products = products
                    .Where(x => productFilterReqModel.Brand
                    .Contains(x.Brand)).ToList();
            }

            if (productFilterReqModel.MinPrice.HasValue)
            {
                products = products
                    .Where(x => x.Price >= productFilterReqModel.MinPrice)
                    .ToList();
            }

            if (productFilterReqModel.MaxPrice.HasValue)
            {
                products = products
                    .Where(x => x.Price <= productFilterReqModel.MaxPrice)
                    .ToList();
            }

            if (productFilterReqModel.MinDiscount.HasValue)
            {
                products = products
                    .Where(x => x.Discount >= productFilterReqModel.MinDiscount)
                    .ToList();
            }

            if (productFilterReqModel.MaxDiscount.HasValue)
            {
                products = products
                    .Where(x => x.Discount <= productFilterReqModel.MaxDiscount)
                    .ToList();
            }


            return products;
        }

    }
}
