using Microsoft.AspNetCore.Http;
using Monytor.Core.Repositories;
using System.Threading.Tasks;

namespace Monytor.WebApi {
    internal class UnitOfWorkMiddleware {
        private readonly RequestDelegate _next;

        public UnitOfWorkMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork) {
            unitOfWork.OpenSession();
            await _next(context);
            if(context.Response.StatusCode < 400) {
                unitOfWork.SaveChanges();
            }
        }
    }
}