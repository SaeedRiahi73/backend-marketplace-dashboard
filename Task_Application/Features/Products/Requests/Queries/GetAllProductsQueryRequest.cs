using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Common.Responses;
using Task_Application.Dtos.Product;
using Task_Domain.Entities;

namespace Task_Application.Features.Products.Requests.Queries
{
    public class GetAllProductsQueryRequest:IRequest<ResultInfo<IEnumerable<ProductDto>>>
    {
    }
}
