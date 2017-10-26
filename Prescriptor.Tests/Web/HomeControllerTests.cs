using Microsoft.AspNetCore.Mvc;
using Prescriptor.Web.Controllers;
using Xunit;

namespace Prescriptor.Tests.Web
{
    public class HomeControllerTests
    {
        [Fact(DisplayName = "Index Should Be Of Type ViewResult")]
        public void Index_ShouldBeOfType_ViewResult()
        {
            var controller = new HomeController();

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }
    }
}
