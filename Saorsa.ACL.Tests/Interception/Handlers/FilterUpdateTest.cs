using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Saorsa.ACL.Tests.Interception.Handlers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using Saorsa.ACL.Interception;
    using Saorsa.ACL.Interception.EF.Model;
    using Saorsa.ACL.Interception.Exceptions;
    using Saorsa.ACL.Interception.Handlers.Attributes;
    using Saorsa.ACL.Model;

    public class MyUpdateService : IMyService
    {
        public string UserId { get; set; }
        public string GetEntityAcl<T>(T entity)
        {
            throw new NotImplementedException();
        }

        public ACLUser GetUser()
        {
            if(string.IsNullOrEmpty(UserId))
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
            return new ACLUser
            {
                Id = "2",
                Groups = new[]
                                {
                                    new ACLGroup
                                    {
                                        Id = 3
                                    },
                                    new ACLGroup
                                    {
                                        Id = 4
                                    },
                                }
            };
        }

        public MyDomainObject GetAllowed()
        {
            throw new NotImplementedException();
        }

        public MyDomainObject GetNotAllowed()
        {
            throw new NotImplementedException();
        }

        public IQueryable<MyDomainObject> GetAllAsIQueryable()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MyDomainObject> GetAllAsIEnumerable()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MyDomainObject> GetAllAsIEnumerable_FiveHundredThousandRecords()
        {
            throw new NotImplementedException();
        }
        [FilterUpdate]
        public void UpdateNoArguments(MyDomainObject obj)
        {
        }
        [FilterUpdate(new []{"controlled"})]
        public void UpdateArgumentsList(MyDomainObject notControlled, MyDomainObject controlled)
        {
        }
        [FilterUpdate(new[] { "blabla" })]
        public void UpdateArgumentsListWrongArgumentName(MyDomainObject obj, MyDomainObject notControlled)
        {
        }
        [FilterUpdate(new[] { "controlled" })]
        public void UpdateArgumentsListWrongArgumentType(object obj, object controlled)
        {
        }
    }

    [TestClass]
    public class FilterUpdateTest
    {
        private IUnityContainer _container;

        [TestInitialize]
        public void Initialize()
        {
            _container = new UnityContainer();
            _container.AddNewExtension<Interception>();
            _container.RegisterType<IMyService, MyUpdateService>().Configure<Interception>()
                .SetInterceptorFor<IMyService>(new InterfaceInterceptor());
        }
        [TestMethod]
        public void Check_UpdateAllowed_NoArgumentsList()
        {
            var service = _container.Resolve<IMyService>();
            service.UpdateNoArguments(new MyDomainObject
            {
                Acls = new List<ACL>
                                      {
                                          new ACL
                                          {
                                              Action = ACLAction.Read,
                                              U = new List<AclUser>
                                                      {
                                                          new AclUser("1")
                                                      },
                                              G = new List<AclGroup>
                                                       {
                                                           new AclGroup
                                                           {
                                                               Id = 2
                                                           }
                                                       }
                                          },
                                          new ACL
                                          {
                                              Action = ACLAction.Write,
                                              U = new List<AclUser>
                                                      {
                                                          new AclUser("1")
                                                      },
                                              G = new List<AclGroup>
                                                       {
                                                           new AclGroup
                                                           {
                                                               Id = 2
                                                           }
                                                       }
                                          }
                                      }
            });
            //No exception expected
            Assert.IsTrue(true);
        }
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Check_ArgumentsList_Wrong_Argument_Name_Exception_Should_Be_Raized()
        {
            var service = _container.Resolve<IMyService>();
            var domainObject = new MyDomainObject();
            service.UpdateArgumentsListWrongArgumentName(domainObject, domainObject);
        }
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Check_ArgumentsList_Argument_Is_Not_AclBase_Exception_Should_Be_Raized()
        {
            var service = _container.Resolve<IMyService>();
            service.UpdateArgumentsListWrongArgumentType(new StringBuilder(), new object());
        }
        [ExpectedException(typeof(AclSecurityException))]
        [TestMethod]
        public void Check_UpdateDenied_NoArgumentsList()
        {
            var service = _container.Resolve<IMyService>();
            service.UserId = "deny";
            service.UpdateNoArguments(new MyDomainObject
                           {
                               Acls = new List<ACL>
                                      {
                                          new ACL
                                          {
                                              Action = ACLAction.Read,
                                              U = new List<AclUser>
                                                      {
                                                          new AclUser("1")
                                                      },
                                              G = new List<AclGroup>
                                                       {
                                                           new AclGroup
                                                           {
                                                               Id = 2
                                                           }
                                                       }
                                          },
                                          new ACL
                                          {
                                              Action = ACLAction.Write,
                                              U = new List<AclUser>
                                                      {
                                                          new AclUser("8")
                                                      },
                                              G = new List<AclGroup>
                                                       {
                                                           new AclGroup
                                                           {
                                                               Id = 9
                                                           }
                                                       }
                                          }
                                      }
                           });
            //No exception expected
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void Check_UpdateAllowed_ArgumentsList()
        {
            var service = _container.Resolve<IMyService>();
            #region Domain Object Definition
            var domainObject = new MyDomainObject
                               {
                                   Acls = new List<ACL>
                                          {
                                              new ACL
                                              {
                                                  Action = ACLAction.Read,
                                                  U = new List<AclUser>
                                                          {
                                                              new AclUser("1")
                                                          },
                                                  G = new List<AclGroup>
                                                           {
                                                               new AclGroup
                                                               {
                                                                   Id = 2
                                                               }
                                                           }
                                              },
                                              new ACL
                                              {
                                                  Action = ACLAction.Write,
                                                  U = new List<AclUser>
                                                          {
                                                              new AclUser("1")
                                                          },
                                                  G = new List<AclGroup>
                                                           {
                                                               new AclGroup
                                                               {
                                                                   Id = 2
                                                               }
                                                           }
                                              }
                                          }
                               };
            #endregion
            service.UpdateArgumentsList(domainObject, domainObject);
            //No exception expected
            Assert.IsTrue(true);
        }
        [ExpectedException(typeof(AclSecurityException))]
        [TestMethod]
        public void Check_UpdateDenied_ArgumentsList()
        {
            var service = _container.Resolve<IMyService>();
            #region Domain Object Definition
            var domainObject = new MyDomainObject
            {
                Acls = new List<ACL>
                                          {
                                              new ACL
                                              {
                                                  Action = ACLAction.Read,
                                                  U = new List<AclUser>
                                                          {
                                                              new AclUser("1")
                                                          },
                                                  G = new List<AclGroup>
                                                           {
                                                               new AclGroup
                                                               {
                                                                   Id = 2
                                                               }
                                                           }
                                              },
                                              new ACL
                                              {
                                                  Action = ACLAction.Write,
                                                  U = new List<AclUser>
                                                          {
                                                              new AclUser("7")
                                                          },
                                                  G = new List<AclGroup>
                                                           {
                                                               new AclGroup
                                                               {
                                                                   Id = 8
                                                               }
                                                           }
                                              }
                                          }
            };
            #endregion
            service.UpdateArgumentsList(domainObject, domainObject);
            //No exception expected
            Assert.IsTrue(true);
        }
    }
}
