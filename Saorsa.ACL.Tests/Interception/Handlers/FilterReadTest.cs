using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Saorsa.ACL.Tests.Interception.Handlers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using Saorsa.ACL.Interception.EF.Model;
    using Saorsa.ACL.Interception.Handlers.Attributes;
    using Saorsa.ACL.Model;
    public class MyReadService : IMyService
    {
        public string UserId { get; set; }
        public string GetEntityAcl<T>(T entity)
        {
            throw new NotImplementedException();
        }

        public ACLUser GetUser()
        {
            return new ACLUser
                   {
                       Id = "1",
                       Groups = new[]
                                {
                                    new ACLGroup
                                    {
                                        Id = 1
                                    },
                                    new ACLGroup
                                    {
                                        Id = 2
                                    },
                                }
                   };
        }
        [FilterRead]
        public MyDomainObject GetAllowed()
        {
            return new MyDomainObject
                   {
                       Name = "My allowed domain object",
                       Acls = new List<ACL>
                              {
                                  new ACL(ACLAction.Read, new List<AclUser>
                                                          {
                                                              new AclUser("1")
                                                          }, null),
                                                          new ACL(ACLAction.Write, new List<AclUser>
                                                          {
                                                              new AclUser("1")
                                                          }, null)
                              }
                   };
        }
        [FilterRead]
        public MyDomainObject GetNotAllowed()
        {
            return new MyDomainObject
            {
                Name = "This should not be returned",
                Acls = new List<ACL>
                              {
                                  new ACL(ACLAction.Read, new List<AclUser>
                                                          {
                                                              new AclUser("7")
                                                          }, null)
                              }
            };
        }
        [FilterRead(typeof(MyDomainObject))]
        public IQueryable<MyDomainObject> GetAllAsIQueryable()
        {
            return new List<MyDomainObject>
                   {
                       GetAllowed(),
                       GetNotAllowed()
                   }.AsQueryable();
        }
        [FilterRead(typeof(MyDomainObject))]
        public IEnumerable<MyDomainObject> GetAllAsIEnumerable()
        {
            return new List<MyDomainObject>
                   {
                       GetAllowed(),
                       GetNotAllowed()
                   }.AsQueryable();
        }
        [FilterRead(typeof(MyDomainObject))]
        public IEnumerable<MyDomainObject> GetAllAsIEnumerable_FiveHundredThousandRecords()
        {
            var list = new List<MyDomainObject>(10000);
            for (int i = 0; i < 249999; i++)
            {
                list.Add(GetAllowed());
            }
            for (int i = 0; i < 250000; i++)
            {
                list.Add(GetNotAllowed());
            }
            return list;
        }

        public void UpdateNoArguments(MyDomainObject obj)
        {
            throw new NotImplementedException();
        }

        public void UpdateArgumentsList(MyDomainObject obj, MyDomainObject notControlled)
        {
            throw new NotImplementedException();
        }

        public void UpdateArgumentsListWrongArgumentName(MyDomainObject obj, MyDomainObject notControlled)
        {
            throw new NotImplementedException();
        }

        public void UpdateArgumentsListWrongArgumentType(object obj, object controlled)
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class FilterReadTest
    {
        private IUnityContainer _container;

        [TestInitialize]
        public void Initialize()
        {
            _container = new UnityContainer();
            _container.AddNewExtension<Interception>();
            _container.RegisterType<IMyService, MyReadService>().Configure<Interception>()
                .SetInterceptorFor<IMyService>(new InterfaceInterceptor());
        }
        [TestMethod]
        public void Filter_Single_Return_Method_Should_Returned_Allowed_ACL()
        {
            var service = _container.Resolve<IMyService>();
            Assert.IsNotNull(service.GetAllowed());
            Assert.IsNull(service.GetNotAllowed());
        }
        [TestMethod]
        public void Filter_IQueryable_Return_Method_Should_Returned_Allowed_ACL()
        {
            var service = _container.Resolve<IMyService>();
            var returnResult = service.GetAllAsIQueryable();
            Assert.IsNotNull(returnResult);
            Assert.AreEqual(1, returnResult.Count());
            Assert.AreEqual("My allowed domain object", returnResult.First().Name);
        }
        [TestMethod]
        public void Filter_IEnumerable_Return_Method_Should_Returned_Allowed_ACL()
        {
            var service = _container.Resolve<IMyService>();
            var returnResult = service.GetAllAsIEnumerable();
            Assert.IsNotNull(returnResult);
            Assert.AreEqual(1, returnResult.Count());
            Assert.AreEqual("My allowed domain object", returnResult.First().Name);
        }
        [TestMethod]
        public void Check_Performance_For_OneHundredThousandRecords()
        {
            var service = _container.Resolve<IMyService>();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var returnResult = service.GetAllAsIEnumerable_FiveHundredThousandRecords();
            stopWatch.Stop();
            Trace.WriteLine("Time elapsed for half a million records: " + stopWatch.Elapsed);
            Assert.IsNotNull(returnResult);
            Assert.AreEqual(249999, returnResult.Count());
            Assert.AreEqual("My allowed domain object", returnResult.First().Name);
        }
    }
}
