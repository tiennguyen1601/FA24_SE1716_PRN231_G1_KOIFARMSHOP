using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Data.Repository;
using KOIFARMSHOP.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KOIFARMSHOP.Data;
using KOIFARMSHOP.Service.Base;
using Attribute = KOIFARMSHOP.Data.Models.Attribute;

namespace KOIFARMSHOP.Service.Services
{
    public interface IAttributeService
    {
        Task<IBusinessResult> GetAllAsync();
        Task<IBusinessResult> GetByIdAsync(int id);
        Task<IBusinessResult> SaveAsync(Attribute attribute);
        Task<IBusinessResult> DeleteByIdAsync(int id);
        Task<Attribute> GetAttributeById(int attributeId);
    }

    public class AttributeService : IAttributeService
    {
        private readonly UnitOfWork _unitOfWork;

        public AttributeService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBusinessResult> GetAllAsync()
        {
            var attributes = await _unitOfWork.AttributeRepository.GetAllAsync();
            if (attributes == null || !attributes.Any())
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, "No attributes found.", new List<Attribute>());
            }
            return new BusinessResult(Const.SUCCESS_READ_CODE, "Attributes retrieved successfully.", attributes);
        }

        public async Task<IBusinessResult> GetByIdAsync(int id)
        {
            var attribute = await _unitOfWork.AttributeRepository.GetByIdAsync(id);
            if (attribute == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, "Attribute not found.", null);
            }
            return new BusinessResult(Const.SUCCESS_READ_CODE, "Attribute retrieved successfully.", attribute);
        }

        public async Task<IBusinessResult> SaveAsync(Attribute attribute)
        {
            try
            {
                int result;
                var existingAttribute = await _unitOfWork.AttributeRepository.GetByIdAsync(attribute.AttributeId);

                if (existingAttribute != null)
                {
                    // Update the existing attribute
                    existingAttribute.AttributeName = attribute.AttributeName;
                    existingAttribute.DisplayName = attribute.DisplayName;
                    existingAttribute.IsComparable = true;
                    existingAttribute.IsDeletable = true;

                    result = await _unitOfWork.AttributeRepository.UpdateAsync(existingAttribute);

                    return result > 0
                        ? new BusinessResult(Const.SUCCESS_UPDATE_CODE, "Attribute updated successfully.", existingAttribute)
                        : new BusinessResult(Const.FAIL_UPDATE_CODE, "Failed to update attribute.", existingAttribute);
                }
                else
                {
                    // Create a new attribute
                    result = await _unitOfWork.AttributeRepository.CreateAsync(attribute);

                    return result > 0
                        ? new BusinessResult(Const.SUCCESS_CREATE_CODE, "Attribute created successfully.", attribute)
                        : new BusinessResult(Const.FAIL_CREATE_CODE, "Failed to create attribute.", attribute);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> DeleteByIdAsync(int id)
        {
            try
            {
                var attribute = await _unitOfWork.AttributeRepository.GetByIdAsync(id);
                if (attribute == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, "Attribute not found.", null);
                }

                var result = await _unitOfWork.AttributeRepository.RemoveAsync(attribute);
                return result
                    ? new BusinessResult(Const.SUCCESS_DELETE_CODE, "Attribute deleted successfully.", attribute)
                    : new BusinessResult(Const.FAIL_DELETE_CODE, "Failed to delete attribute.", attribute);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<Attribute?> GetAttributeById(int attributeId)
        {
            return await _unitOfWork.AttributeRepository.GetByIdAsync(attributeId);
        }

    }
}
