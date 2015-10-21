using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Saorsa.ACL.Tests.Interception.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using ACL.Interception.EF.Model;
    using Saorsa.ACL.Interception.Exceptions;
    using Saorsa.ACL.Interception.Handlers.Attributes;
    using Saorsa.ACL.Model;

    public class MyUpdateService :  IMyService
    {
        public string UserId { get; set; }
       

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
                                    }
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
                                    }
                                }
            };
        }

        public AclDomainObject GetAllowed()
        {
            throw new NotImplementedException();
        }

        public AclDomainObject GetNotAllowed()
        {
            throw new NotImplementedException();
        }

        public IQueryable<AclDomainObject> GetAllAsIQueryable()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AclDomainObject> GetAllAsIEnumerable()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AclDomainObject> GetAllAsIEnumerable_FiveHundredThousandRecords()
        {
            throw new NotImplementedException();
        }
        [FilterUpdate]
        public void UpdateNoArguments(AclDomainObject obj)
        {
        }
        [FilterUpdate(new []{"controlled"})]
        public void UpdateArgumentsList(AclDomainObject notControlled, AclDomainObject controlled)
        {
        }
        [FilterUpdate(new[] { "blabla" })]
        public void UpdateArgumentsListWrongArgumentName(AclDomainObject obj, AclDomainObject notControlled)
        {
        }
        [FilterUpdate(new[] { "controlled" })]
        public void UpdateArgumentsListWrongArgumentType(object obj, object controlled)
        {
        }
        [FilterUpdate(new[] { "domainObjects" })]
        public void UpdateEnumerableArgumentsList(IEnumerable<AclDomainObject> domainObjects)
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
            service.UpdateNoArguments(new AclDomainObject
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
            var domainObject = new AclDomainObject();
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
            service.UpdateNoArguments(new AclDomainObject
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
            var domainObject = new AclDomainObject
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
            var domainObject = new AclDomainObject
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
        [TestMethod]
        public void Check_UpdateAllowed_EnumerableArgumentsList()
        {
            var service = _container.Resolve<IMyService>();
            #region Domain Object Definition
            var domainObject = new AclDomainObject
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
                                                                   Id = 2
                                                               }
                                                           }
                                              }
                                          }
            };
            #endregion
            service.UpdateEnumerableArgumentsList(new List<AclDomainObject> { domainObject, domainObject });
            //No exception expected
            Assert.IsTrue(true);
        }
        [ExpectedException(typeof(AclSecurityException))]
        [TestMethod]
        public void Check_UpdateNotAllowed_EnumerableArgumentsList()
        {
            var service = _container.Resolve<IMyService>();
            #region Domain Object Definition
            var domainObject = new AclDomainObject
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
                                                                   Id = 6
                                                               }
                                                           }
                                              }
                                          }
            };
            #endregion
            service.UpdateEnumerableArgumentsList(new List<AclDomainObject> { domainObject, domainObject });
        }
    }
}
