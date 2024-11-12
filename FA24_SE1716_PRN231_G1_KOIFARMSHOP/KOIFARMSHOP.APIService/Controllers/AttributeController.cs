using System.Collections.Generic;
using System.Threading.Tasks;
using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data.DTO.AttributeDTO;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Attribute = KOIFARMSHOP.Data.Models.Attribute;

namespace KOIFARMSHOP.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeController : ControllerBase
    {
        private readonly IAttributeService _attributeService;

        public AttributeController(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        // GET: api/Attribute
        [HttpGet]
        public async Task<IBusinessResult> GetAttributes()
        {
            return await _attributeService.GetAllAsync();
        }

        // GET: api/Attribute/{attributeId}
        [HttpGet("{attributeId}")]
        public async Task<IBusinessResult> ViewDetails(int attributeId)
        {
            return await _attributeService.GetByIdAsync(attributeId);
        }

        // POST: api/Attribute
        [HttpPost]
        public async Task<IBusinessResult> CreateAttribute(AttributeCreateReqModel attributeCreateReqModel)
        {
            Attribute newAttribute = new Attribute
            {
                AttributeName = attributeCreateReqModel.AttributeName,
                DisplayName = attributeCreateReqModel.DisplayName,
                IsComparable = true,
                IsDeletable = true
            };

            var result = await _attributeService.SaveAsync(newAttribute);

            return result;
        }

        // PUT: api/Attribute
        [HttpPut]
        public async Task<IBusinessResult> UpdateAttribute(AttributeUpdateReqModel attributeUpdateReqModel)
        {
            var currAttribute = await _attributeService.GetAttributeById(attributeUpdateReqModel.AttributeId);

            if (currAttribute == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Attribute>());
            }

            currAttribute.AttributeName = !string.IsNullOrEmpty(attributeUpdateReqModel.AttributeName) ? attributeUpdateReqModel.AttributeName : currAttribute.AttributeName;
            currAttribute.DisplayName = !string.IsNullOrEmpty(attributeUpdateReqModel.DisplayName) ? attributeUpdateReqModel.DisplayName : currAttribute.DisplayName;
        
            var result = await _attributeService.SaveAsync(currAttribute);

            return result;
        }

        // DELETE: api/Attribute/{attributeId}
        [HttpDelete("{attributeId}")]
        public async Task<IBusinessResult> DeleteAttribute(int attributeId)
        {
            return await _attributeService.DeleteByIdAsync(attributeId);
        }

    }
}
