//using OpenQA.Selenium;
//using OpenQA.Selenium.Support.UI;
//using Xunit;

//namespace WebMVC.Tests.SeleniumTests
//{
//    public class WebMVCTests : TestBase
//    {
//        private readonly WebDriverWait _wait;
//        private const string BaseUrl = "https://localhost:7203";

//        public WebMVCTests()
//        {
//            _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
//        }

//        [RetryFact(MaxRetries = 3)]
//        public void UserJourney_HomePageNavigation()
//        {
//            try
//            {
//                // 1. Truy cập trang chủ
//                Driver.Navigate().GoToUrl(BaseUrl);
//                Assert.Contains("WebShoppeFood", Driver.Title);

//                // 2. Kiểm tra menu chính
//                var mainMenu = _wait.Until(d => d.FindElement(By.ClassName("navbar-nav")));
//                Assert.True(mainMenu.Displayed);

//                // 3. Kiểm tra các link chính
//                var menuItems = Driver.FindElements(By.CssSelector(".navbar-nav .nav-link"));
//                Assert.Contains(menuItems, item => item.Text.Contains("Home"));
//                Assert.Contains(menuItems, item => item.Text.Contains("Menu"));
//                Assert.Contains(menuItems, item => item.Text.Contains("About"));
//                Assert.Contains(menuItems, item => item.Text.Contains("Contact"));

//                // 4. Kiểm tra slider
//                var slider = _wait.Until(d => d.FindElement(By.ClassName("carousel")));
//                Assert.True(slider.Displayed);

//                // 5. Kiểm tra featured products
//                var featuredProducts = _wait.Until(d => d.FindElements(By.ClassName("featured-product")));
//                Assert.True(featuredProducts.Count > 0);
//            }
//            catch (Exception ex)
//            {
//                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
//                screenshot.SaveAsFile($"test-failure-{DateTime.Now:yyyyMMddHHmmss}.png");
//                throw;
//            }
//        }

//        [RetryFact(MaxRetries = 3)]
//        public void UserJourney_ProductDetails()
//        {
//            try
//            {
//                // 1. Truy cập trang menu
//                Driver.Navigate().GoToUrl($"{BaseUrl}/Menu");
//                Assert.Contains("Menu", Driver.Title);

//                // 2. Chọn sản phẩm đầu tiên
//                var firstProduct = _wait.Until(d => d.FindElement(By.ClassName("product-item")));
//                firstProduct.Click();

//                // 3. Kiểm tra chi tiết sản phẩm
//                var productDetails = _wait.Until(d => d.FindElement(By.ClassName("product-details")));
//                Assert.True(productDetails.Displayed);

//                // 4. Kiểm tra thông tin sản phẩm
//                var productName = Driver.FindElement(By.ClassName("product-name"));
//                var productPrice = Driver.FindElement(By.ClassName("product-price"));
//                var productDescription = Driver.FindElement(By.ClassName("product-description"));

//                Assert.NotNull(productName.Text);
//                Assert.Contains("$", productPrice.Text);
//                Assert.NotNull(productDescription.Text);

//                // 5. Thêm vào giỏ hàng
//                var addToCartButton = Driver.FindElement(By.ClassName("add-to-cart-button"));
//                addToCartButton.Click();

//                // 6. Kiểm tra thông báo
//                var notification = _wait.Until(d => d.FindElement(By.ClassName("toast")));
//                Assert.True(notification.Displayed);
//                Assert.Contains("Added to cart", notification.Text);
//            }
//            catch (Exception ex)
//            {
//                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
//                screenshot.SaveAsFile($"test-failure-{DateTime.Now:yyyyMMddHHmmss}.png");
//                throw;
//            }
//        }

//        [RetryFact(MaxRetries = 3)]
//        public void UserJourney_CheckoutProcess()
//        {
//            try
//            {
//                // 1. Đăng nhập
//                Driver.Navigate().GoToUrl($"{BaseUrl}/Account/Login");
//                var usernameInput = _wait.Until(d => d.FindElement(By.Id("username")));
//                var passwordInput = Driver.FindElement(By.Id("password"));
//                var loginButton = Driver.FindElement(By.Id("login-button"));

//                usernameInput.SendKeys("test@example.com");
//                passwordInput.SendKeys("Password123!");
//                loginButton.Click();

//                // 2. Thêm sản phẩm vào giỏ hàng
//                Driver.Navigate().GoToUrl($"{BaseUrl}/Menu");
//                var addToCartButton = _wait.Until(d => d.FindElement(By.ClassName("add-to-cart-button")));
//                addToCartButton.Click();

//                // 3. Xem giỏ hàng
//                var cartLink = _wait.Until(d => d.FindElement(By.LinkText("Cart")));
//                cartLink.Click();

//                // 4. Kiểm tra giỏ hàng
//                var cartItems = _wait.Until(d => d.FindElements(By.ClassName("cart-item")));
//                Assert.True(cartItems.Count > 0);

//                // 5. Cập nhật số lượng
//                var quantityInput = Driver.FindElement(By.ClassName("quantity-input"));
//                quantityInput.Clear();
//                quantityInput.SendKeys("2");

//                var updateButton = Driver.FindElement(By.ClassName("update-quantity-button"));
//                updateButton.Click();

//                // 6. Tiến hành thanh toán
//                var checkoutButton = _wait.Until(d => d.FindElement(By.Id("checkout-button")));
//                checkoutButton.Click();

//                // 7. Nhập thông tin giao hàng
//                var addressInput = _wait.Until(d => d.FindElement(By.Id("address")));
//                var phoneInput = Driver.FindElement(By.Id("phone"));
//                var confirmButton = Driver.FindElement(By.Id("confirm-order"));

//                addressInput.SendKeys("123 Test Street");
//                phoneInput.SendKeys("0123456789");
//                confirmButton.Click();

//                // 8. Xác nhận đơn hàng
//                var orderConfirmation = _wait.Until(d => d.FindElement(By.ClassName("order-confirmation")));
//                Assert.True(orderConfirmation.Displayed);
//                Assert.Contains("Thank you for your order", orderConfirmation.Text);
//            }
//            catch (Exception ex)
//            {
//                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
//                screenshot.SaveAsFile($"test-failure-{DateTime.Now:yyyyMMddHHmmss}.png");
//                throw;
//            }
//        }

//        [RetryFact(MaxRetries = 3)]
//        public void UserJourney_UserProfile()
//        {
//            try
//            {
//                // 1. Đăng nhập
//                Driver.Navigate().GoToUrl($"{BaseUrl}/Account/Login");
//                var usernameInput = _wait.Until(d => d.FindElement(By.Id("username")));
//                var passwordInput = Driver.FindElement(By.Id("password"));
//                var loginButton = Driver.FindElement(By.Id("login-button"));

//                usernameInput.SendKeys("test@example.com");
//                passwordInput.SendKeys("Password123!");
//                loginButton.Click();

//                // 2. Truy cập profile
//                var profileLink = _wait.Until(d => d.FindElement(By.LinkText("Profile")));
//                profileLink.Click();

//                // 3. Kiểm tra thông tin profile
//                var profileInfo = _wait.Until(d => d.FindElement(By.ClassName("profile-info")));
//                Assert.True(profileInfo.Displayed);

//                // 4. Cập nhật thông tin
//                var updateProfileButton = Driver.FindElement(By.Id("update-profile-button"));
//                updateProfileButton.Click();

//                var nameInput = _wait.Until(d => d.FindElement(By.Id("name")));
//                var phoneInput = Driver.FindElement(By.Id("phone"));
//                var saveButton = Driver.FindElement(By.Id("save-profile-button"));

//                nameInput.Clear();
//                nameInput.SendKeys("New Name");
//                phoneInput.Clear();
//                phoneInput.SendKeys("0987654321");
//                saveButton.Click();

//                // 5. Xác nhận cập nhật
//                var successMessage = _wait.Until(d => d.FindElement(By.ClassName("alert-success")));
//                Assert.True(successMessage.Displayed);
//                Assert.Contains("Profile updated successfully", successMessage.Text);

//                // 6. Xem lịch sử đơn hàng
//                var orderHistoryLink = Driver.FindElement(By.LinkText("Order History"));
//                orderHistoryLink.Click();

//                var orderHistory = _wait.Until(d => d.FindElement(By.ClassName("order-history")));
//                Assert.True(orderHistory.Displayed);
//            }
//            catch (Exception ex)
//            {
//                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
//                screenshot.SaveAsFile($"test-failure-{DateTime.Now:yyyyMMddHHmmss}.png");
//                throw;
//            }
//        }
//    }
//} 