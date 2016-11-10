using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Interface.Web.Controllers;
using Its.Systems.HR.Interface.Web.ViewModels;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Its.Systems.HR.Test.Controllers
{
    [TestClass]
    public class Session : BaseTest
    {
        private readonly IActivityManager _activityManager;
        private readonly ISessionManager _sessionManager;
        private readonly IPersonManager _personManager;
        private readonly IUtilityManager _utilityManager;

        public Session() : base()
        {
            _activityManager = Container().Resolve<IActivityManager>();
            _sessionManager = Container().Resolve<ISessionManager>();
            _personManager = Container().Resolve<IPersonManager>();
            _utilityManager = Container().Resolve<IUtilityManager>();
        }
        

        [TestMethod]
        public void CreateSession_ShouldReturnViewResult()
        {
            // Arrange
            var controller = new SessionController(_activityManager, _sessionManager, _personManager, _utilityManager);

            // Act
            ViewResult result = controller.CreateSession() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        //[TestMethod]
        //public void ActivityIndexWithSearchstringJava_ShouldReturnActivityJavaOne()
        //{
        //    // Arrange
        //    ActivityController controller = new ActivityController(_activityManager, _personManager, _utilityManager);

        //    // Act
        //    var controllerResult = controller.ActivityIndex("Java");

        //    var model = (IndexActivityViewModel) controllerResult.Model;

        //    var result = new List<ActivityWithCountOfSessions>();
        //    foreach (var activity in model.Activities)
        //    {
        //        result.Add(activity);
        //    }
        //    // Assert
        //    Assert.AreEqual("JavaOne", result[0].Name);
        //}

        //[TestMethod]
        //public void CreateActivity_ShouldAddThatActivity()
        //{
        //    // Arrange
        //    ActivityController controller = new ActivityController(_activityManager, _personManager, _utilityManager);

        //    var activityToAdd = new ActivityViewModel()
        //    {
        //        Name = "New Activity"
        //    };

        //    // Act
        //    var controllerResult = controller.CreateActivity(activityToAdd);

        //    var expected = _activityManager.GetAllActivities().SingleOrDefault(n => n.Name == "New Activity");
            
        //    // Assert
        //    Assert.AreEqual(expected.Name, activityToAdd.Name);
        //}

        //[TestMethod]
        //public void CreateActivityWithNoName_ShouldReturnModelStateNotValid()
        //{
        //    var controller = new ActivityController(_activityManager, _personManager, _utilityManager);
            
        //    var model = new ActivityViewModel()
        //    {
        //        Name = ""
        //    };

        //    //Init ModelState
        //    var modelBinder = new ModelBindingContext()
        //    {
        //        ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(
        //                          () => model, model.GetType()),
        //        ValueProvider = new NameValueCollectionValueProvider(
        //                            new NameValueCollection(), CultureInfo.InvariantCulture)
        //    };
        //    var binder = new DefaultModelBinder().BindModel(
        //                     new ControllerContext(), modelBinder);
        //    controller.ModelState.Clear();
        //    controller.ModelState.Merge(modelBinder.ModelState);

        //    var controllerResult = controller.CreateActivity(model);

        //    ViewResult result = (ViewResult)controllerResult;
        //    Assert.IsTrue(result.ViewData.ModelState["Name"].Errors.Count > 0);
        //    Assert.IsTrue(!result.ViewData.ModelState.IsValid);
        //}

        [TestMethod]
        public void CreateSessionWithInvalidModel_ShouldReturnModelStateNotValid()
        {
            var controller = new SessionController(_activityManager, _sessionManager, _personManager, _utilityManager);

            var model = new CreateSessionViewModel()
            {
                Activity = null,
                Name = ""
            };

            //Init ModelState
            var modelBinder = new ModelBindingContext()
            {
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(
                                  () => model, model.GetType()),
                ValueProvider = new NameValueCollectionValueProvider(
                                    new NameValueCollection(), CultureInfo.InvariantCulture)
            };
            var binder = new DefaultModelBinder().BindModel(
                             new ControllerContext(), modelBinder);
            controller.ModelState.Clear();
            controller.ModelState.Merge(modelBinder.ModelState);

            ViewResult result = (ViewResult)controller.CreateSession(model);
            Assert.IsTrue(result.ViewData.ModelState["Activity"].Errors.Count > 0);
            Assert.IsTrue(result.ViewData.ModelState["Name"].Errors.Count > 0);
            Assert.IsTrue(!result.ViewData.ModelState.IsValid);
        }


        //[TestMethod]
        //public void Verify_ActivityIndex_Method_Is_Decorated_With_Authorize_Attribute()
        //{
        //    ActivityController controller = new ActivityController(_activityManager, _personManager, _utilityManager);
        //    var type = controller.GetType();
        //    var methodInfo = type.GetMethod("ActivityIndex", new Type[] { typeof(string) });
        //    var attributes = methodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);
        //    Assert.IsTrue(attributes.Any(), "No AuthorizeAttribute found on ActivityIndex(string searchString) method");
        //}
    }
}
