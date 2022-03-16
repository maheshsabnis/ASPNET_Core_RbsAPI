

namespace RbsAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersService repository;
        private readonly AuthSecurityService service;

        public OrdersController(OrdersService repository, AuthSecurityService service)
        {
            this.repository = repository;
            this.service = service;
        }

        [HttpGet] 
        [ActionName("orders")]
        [Authorize(Policy = "AdminManagerClerkPolicy")]
        public async Task<IActionResult> Get()
        {
            try
            {
                GetRequestInfo(Request, out string userName, out string roleName);
                var orders = await repository.GetAsync();
                if (roleName == "Administrator")
                {
                    return Ok(orders);
                }
                var ordersByUserName = orders.Where(ord => ord.CreatedBy == userName.Trim());
                return Ok(ordersByUserName);
            }
            catch (Exception ex)
            {
                Response.Headers.Add("Error", ex.Message);
                return NoContent();
            }
        }
        [HttpGet("{id}")]
        [ActionName("orders")]
        [Authorize(Policy = "AdminManagerClerkPolicy")]
        public async  Task<IActionResult> Get(int id)
        {
            try
            {
                Orders order = await repository.GetAsync(id);
                if (order == null)
                {
                    return NotFound($"Record based on {id} is not found.");
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                Response.Headers.Add("Error", ex.Message);
                return NoContent();
            }
        }

        // Orders can be created by Administrator, Manager and Clerk

        [HttpPost]
        [ActionName("saveorder")]
        [Authorize(Policy = "AdminManagerClerkPolicy")]
        public async Task<IActionResult> Post(Orders orders)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GetRequestInfo(Request, out string userName, out string roleName);
                    orders.CreatedDate = DateTime.Today;
                    orders.CreatedBy = userName;
                    orders.UpdatedBy = userName;
                    orders = await repository.CreateAsync(orders);
                    return Ok(orders);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                Response.Headers.Add("Error", ex.Message);
                return NoContent();
            }
        }

        // Non- Approved Orders can be updated by Administrator, Manager and Clerk
        [HttpPut("{id}")]
        [ActionName("updateorder")]
        [Authorize(Policy = "AdminManagerClerkPolicy")]
        public async Task<IActionResult> Put(int id, Orders orders)
        {
            try
            {
                if (id != orders.OrderUniqueId)
                {
                    return Conflict($"The headers {id} does not match with {orders.OrderUniqueId}");
                }
                if (ModelState.IsValid)
                {
                    GetRequestInfo(Request, out string userName, out string roleName);
                    orders.UpdatedDate = DateTime.Today;
                    orders.UpdatedBy = userName;
                    var response = await repository.UpdateAsync(id,orders);
                    if(response) return Ok(orders);
                    return NotFound($"Record based on {id} is not found.");
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                Response.Headers.Add("Error", ex.Message);
                return NoContent();
            }
        }


        // Non- Approved Orders can be updated by Administrator, Manager  
        [HttpPut("{id}")]
        [ActionName("approveorder")]
        [Authorize(Policy = "AdminManagerPolicy")]
        public async Task<IActionResult> Approve(int id, Orders ord)
        {
            try
            {
                var order = await repository.GetAsync(id);
                if (order == null)
                {
                    return NotFound("Record Not found");
                }
                if (order.IsOrderApproved)
                {
                    Response.Headers.Add("Response ", "Order is already approved");
                    return NoContent();
                }
                var res = await repository.ApproveOrderAsync(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                Response.Headers.Add("Error", ex.Message);
                return NoContent();
            }
        }


        [HttpDelete("{id}")]
        [ActionName("deleteorder")]
        [Authorize(Policy = "AdminManagerPolicy")]
        public IActionResult Delete(int id)
        {
            try
            {
                var response = repository.DeleteAsync(id).Result;
                if (response) return Ok(response);
                return NotFound($"Record based on {id} is not found.");
            }
            catch (Exception ex)
            {
                Response.Headers.Add("Error", ex.Message);
                return NoContent();
            }
        }


        private void GetRequestInfo(HttpRequest request, out string userName, out string roleName)
        {
            var headers = request.Headers["Authorization"];
            var receivedToken = headers[0].Split(" ");
            userName =  service.GetUserFromTokenAsync(receivedToken[1]).Result;
            roleName = service.GetRoleFormToken(receivedToken[1]);
        }
    }
}