using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Base;
using System;
using System.Collections.Generic;
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
        Task<IBusinessResult> Save(Product product);
        Task<IBusinessResult> DeleteById(int productId);
    }
    public class ProductService : IProductService
    {
        private readonly UnitOfWork _unitOfWork;

        public ProductService()
        {
            _unitOfWork ??= new UnitOfWork();
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
                    var result = await _unitOfWork.ProductRepository.RemoveAsync(currProduct);

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
            var products = await _unitOfWork.ProductRepository.GetAllAsync();

            if (products == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Product>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, products);
            }
        }

        public Task<IBusinessResult> GetAll(int? page, int? size)
        {
            throw new NotImplementedException();
        }

        public async Task<IBusinessResult> GetById(int productId)
        {
            var currProduct = await _unitOfWork.ProductRepository.GetByIdAsync(productId);

            if (currProduct == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Product>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, currProduct);
            }
        }

        public async Task<IBusinessResult> Save(Product product)
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
    }
}
