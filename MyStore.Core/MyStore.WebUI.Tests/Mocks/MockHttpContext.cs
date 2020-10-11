using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyStore.WebUI.Tests.Mocks
{
    //MockContext is implementing HttpContextBase class
    //the actual HttpContextBase is written in such a way that the default implementation
    //doesn't actually implement anything because this is more of an abstract clustered we can use as our base.
    //Rather like I used the T entity class. It means I can use HttpContextBase as our base and 
    //implement that and then simply override the methods that I want to in our case. I want to override 
    //the question response methods and so that I can expose as a collection of cookies. Because I want to
    //be able to override the request in response. I also need to create individual classes to
    //create mock requests and mock responses in the same file created a new classes as mock response and mockRequest.
    public class MockHttpContext : HttpContextBase
    {
        //implementing response and request classes in actual base class
        private MockRequest request;
        private MockResponse response;
        private HttpCookieCollection cookies;
        private IPrincipal FakeUser; //(linking customers to orders process)

        //constructor
        public MockHttpContext()
        {
            //initialized everything that I need.
            //Creating cookies and passing through each of these requests
            //when our classes reading writing cookies it needs to work with the same underlying list.
            cookies = new HttpCookieCollection();
            this.request = new MockRequest(cookies);
            this.response = new MockResponse(cookies);
        }
        //Create an override that returns our FakeUser.(linking customers to orders process)
        public override IPrincipal User {
            get
            {
                return this.FakeUser; //when I try to get that will return this start FakeUser.
            }
            set
            {
                this.FakeUser = value; //I also need to allow me to set the user so that I can simulate somebody who is locked in.
            }
        }
        //Created some override methods to actually return our overwritten request of responses.
        public override HttpRequestBase Request
        {
            get
            {
                return request;
            }
        }

        public override HttpResponseBase Response
        {
            get
            {
                return response;
            }
        }
    }
    //HttpResponseBase I will override the methods that exposes the underlying cookies collection.
    public class MockResponse : HttpResponseBase
    {
        private readonly HttpCookieCollection cookies;
        //constructor that allows me to pass in the cookie collection
        //I can have direct control over the cookies within our tests.
        public MockResponse(HttpCookieCollection cookies)
        {
            this.cookies = cookies;
        }
        //Expose that I use the override key words.
        //This allows me to override the base methods of the HttpResponseBase.
        public override HttpCookieCollection Cookies
        {
            //Rather than using the base implementation Instead it will return private list of cookies.
            get
            {
                return cookies;
            }
        }
    }
    // Everything is almost same with MockRespinse class
    public class MockRequest : HttpRequestBase
    {
        private readonly HttpCookieCollection cookies;

        public MockRequest(HttpCookieCollection cookies)
        {
            this.cookies = cookies;
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return cookies;
            }
        }
    }

}