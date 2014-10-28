namespace Saorsa.ACL.Tests.Interception.EF
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Saorsa.ACL.Interception.EF.Model;
    using Saorsa.ACL.Model;

    [TestClass]
    public class AclBaseTests
    {
        private AclBase _aclBase = new AclBase();
        [TestInitialize]
        public void Initialize()
        {
           this._aclBase = new AclBase
                                  {
                                      Acls = new List<ACL>
                                             {
                                                 new ACL(ACLAction.Read,
                                                         new List<AclUser>
                                                         {
                                                             new AclUser("1"),
                                                             new AclUser("2"),
                                                             new AclUser("3"),
                                                         },
                                                         new List<AclGroup>
                                                         {
                                                             new AclGroup
                                                             {
                                                                 Id = 1
                                                             },
                                                             new AclGroup
                                                             {
                                                                 Id = 2
                                                             },
                                                             new AclGroup
                                                             {
                                                                 Id = 3
                                                             }
                                                         }),
                                                 new ACL(ACLAction.Write,
                                                         new List<AclUser>
                                                         {
                                                             new AclUser("1"),
                                                             new AclUser("5"),
                                                             new AclUser("6"),
                                                         },
                                                         new List<AclGroup>
                                                         {
                                                             new AclGroup
                                                             {
                                                                 Id = 1
                                                             },
                                                             new AclGroup
                                                             {
                                                                 Id = 4
                                                             },
                                                             new AclGroup
                                                             {
                                                                 Id = 5
                                                             }
                                                         })
                                             }
                                  };
        }

        [TestMethod]
        public void Test_ACL_Serialization()
        {
            Assert.AreEqual(_aclBase.Acl,"[{\"Action\":0,\"Groups\":[{\"Id\":1},{\"Id\":2},{\"Id\":3}],\"Users\":[{\"Id\":\"1\"},{\"Id\":\"2\"},{\"Id\":\"3\"}]},{\"Action\":1,\"Groups\":[{\"Id\":1},{\"Id\":4},{\"Id\":5}],\"Users\":[{\"Id\":\"1\"},{\"Id\":\"5\"},{\"Id\":\"6\"}]}]");
        }
        [TestMethod]
        public void Test_ACL_Cleanup()
        {
            #region Test execution
            var cleanACL = AclBase.CleanUpAcl(new List<ACL>
                                              {
                                                  //The two read and write items should be merged
                                                  new ACL(ACLAction.Read,
                                                          new List<AclUser>
                                                          {
                                                              new AclUser("1"),
                                                              new AclUser("2"),
                                                              new AclUser("3"),
                                                          },
                                                          new List<AclGroup>
                                                          {
                                                              new AclGroup
                                                              {
                                                                  Id = 1
                                                              },
                                                              new AclGroup
                                                              {
                                                                  Id = 2
                                                              },
                                                              new AclGroup
                                                              {
                                                                  Id = 3
                                                              }
                                                          }),
                                                  new ACL(ACLAction.Read,
                                                          new List<AclUser>
                                                          {
                                                              new AclUser("1"),
                                                              new AclUser("6"),
                                                              new AclUser("3"),
                                                          },
                                                          new List<AclGroup>
                                                          {
                                                              new AclGroup
                                                              {
                                                                  Id = 1
                                                              },
                                                              new AclGroup
                                                              {
                                                                  Id = 5
                                                              },
                                                              new AclGroup
                                                              {
                                                                  Id = 3
                                                              }
                                                          }),
                                                  //The 3 write actions should be merged as one
                                                  new ACL(ACLAction.Write,
                                                          new List<AclUser>
                                                          {
                                                              new AclUser("1"),
                                                              new AclUser("2"),
                                                              new AclUser("3"),
                                                          },
                                                          new List<AclGroup>
                                                          {
                                                              new AclGroup
                                                              {
                                                                  Id = 1
                                                              },
                                                              new AclGroup
                                                              {
                                                                  Id = 4
                                                              },
                                                              new AclGroup
                                                              {
                                                                  Id = 5
                                                              }
                                                          }),
                                                      new ACL(ACLAction.Write,
                                                          new List<AclUser>
                                                          {
                                                              new AclUser("1"),
                                                              new AclUser("4"),
                                                              new AclUser("5"),
                                                          },
                                                          new List<AclGroup>
                                                          {
                                                              new AclGroup
                                                              {
                                                                  Id = 1
                                                              },
                                                              new AclGroup
                                                              {
                                                                  Id = 2
                                                              },
                                                              new AclGroup
                                                              {
                                                                  Id = 3
                                                              }
                                                          }),
                                                       new ACL(ACLAction.Write,
                                                          new List<AclUser>
                                                          {
                                                              new AclUser("1"),
                                                              new AclUser("5"),
                                                              new AclUser("6"),
                                                          },
                                                          new List<AclGroup>
                                                          {
                                                              new AclGroup
                                                              {
                                                                  Id = 1
                                                              },
                                                              new AclGroup
                                                              {
                                                                  Id = 6
                                                              },
                                                              new AclGroup
                                                              {
                                                                  Id = 5
                                                              }
                                                          })
                                              });
            #endregion
            Assert.IsNotNull(cleanACL, "Something went wrong there is no cleaned up object");
            Assert.IsNotNull(cleanACL.Count, "The final groups should be two - read and write");
            //Check the read acl
            var readAcl = cleanACL.First(a => a.Action == ACLAction.Read);
            var expectedUserIds = new[] { "1", "2", "3", "6" };
            var expectedGroupIds = new[] { 1, 2, 3, 5 };
            Assert.AreEqual(4, readAcl.Groups.Count, "The read ACL groups count doesn't match.");
            Assert.AreEqual(4, readAcl.Users.Count, "The read ACL users count doesn't match.");
            Assert.IsTrue(readAcl.Users.All(u => expectedUserIds.Any(exp => exp == u.Id)), "The read acl users do not have the expected ids..");
            Assert.IsTrue(readAcl.Groups.All(u => expectedGroupIds.Any(exp => exp == u.Id)), "The read acl groups do not have the expected ids..");

            //Check the write acl
            var writeAcl = cleanACL.First(a => a.Action == ACLAction.Write);
            expectedUserIds = new[] { "1", "2", "3", "6", "5", "4" };
            expectedGroupIds = new[] { 1, 2, 3, 5, 4,6 };
            Assert.AreEqual(6, writeAcl.Groups.Count, "The write ACL groups count doesn't match.");
            Assert.AreEqual(6, writeAcl.Users.Count, "The write ACL users count doesn't match.");
            Assert.IsTrue(writeAcl.Users.All(u => expectedUserIds.Any(exp => exp == u.Id)), "The write acl users do not have the expected ids..");
            Assert.IsTrue(writeAcl.Groups.All(u => expectedGroupIds.Any(exp => exp == u.Id)), "The write acl groups do not have the expected ids..");
        }

        [TestMethod]
        public void Test_Adding_ACLs()
        {
            _aclBase.AddAcl(new ACL(ACLAction.Write, new List<AclUser> {new AclUser("1")}, new List<AclGroup> {new AclGroup {Id = 1}} ));
            Assert.AreEqual(2, _aclBase.Acls.Count,"The add method should not add an additional write acl, nor users or groups, since they are present.");
            Assert.AreEqual(3, _aclBase.Acls.First().Users.Count,"The add method should not add an additional read acl, nor users or groups, since they are present.");
            Assert.AreEqual(3, _aclBase.Acls.First().Groups.Count,"The add method should not add an additional read acl, nor users or groups, since they are present.");
            Assert.AreEqual(3, _aclBase.Acls.Last().Groups.Count,"The add method should not add an additional write acl, nor users or groups, since they are present.");
            Assert.AreEqual(3, _aclBase.Acls.Last().Users.Count,"The add method should not add an additional write acl, nor users or groups, since they are present.");
            //Reuse, since the object should be te same as the initial one.
            Test_ACL_Serialization();
            _aclBase.AddAcl(new ACL(ACLAction.Write, new List<AclUser> { new AclUser("7") }, new List<AclGroup> { new AclGroup { Id = 7 } }));
            Assert.AreEqual(2, _aclBase.Acls.Count, "The add method should not add an additional write acl, nor users or groups, since they are present.");
            Assert.AreEqual(3, _aclBase.Acls.First().Users.Count, "The add method should not add an additional read acl, nor users or groups, since they are present.");
            Assert.AreEqual(3, _aclBase.Acls.First().Groups.Count, "The add method should not add an additional read acl, nor users or groups, since they are present.");
            Assert.AreEqual(4, _aclBase.Acls.Last().Groups.Count, "The add method should not add an additional write acl, nor users or groups, since they are present.");
            Assert.AreEqual(4, _aclBase.Acls.Last().Users.Count, "The add method should not add an additional write acl, nor users or groups, since they are present.");
            //Test empty ACL add.
            var acl = new AclBase();
            acl.AddAcl(new ACL(ACLAction.Write, new List<AclUser> { new AclUser("7") }, new List<AclGroup> { new AclGroup { Id = 7 } }));
            acl.AddAcl(new ACL(ACLAction.Write, new List<AclUser> { new AclUser("7") }, new List<AclGroup> { new AclGroup { Id = 7 } }));
            acl.AddAcl(new ACL(ACLAction.Write, new List<AclUser> { new AclUser("7") }, new List<AclGroup> { new AclGroup { Id = 7 } }));
            Assert.AreEqual(1, acl.Acls.Count, "The add method should add an additional write acl and should add the new user and group once.");
            Assert.AreEqual(1, acl.Acls.First().Users.Count, "The add method should not add an additional write acl and should add the new user and group once.");
            Assert.AreEqual(1, acl.Acls.First().Groups.Count, "The add method should not add an additional write acl and should add the new user and group once.");
            acl.AddAcl(new ACL(ACLAction.Write, new List<AclUser> { new AclUser("1") }, new List<AclGroup> { new AclGroup { Id = 1 } }));
            Assert.AreEqual(1, acl.Acls.Count, "The add method should not add an additional write acl and should add the new user and group once.");
            Assert.AreEqual(2, acl.Acls.First().Users.Count, "The add method should not add an additional write acl and should add the new user and group once");
            Assert.AreEqual(2, acl.Acls.First().Groups.Count, "The add method should not add an additional write acl, nor users or groups and should add the new user and group once");
        }

        [TestMethod]
        public void Test_ACBase_ACL_Removal()
        {
           _aclBase.RemoveAcl(new ACL(ACLAction.Read,new List<AclUser>{new AclUser("1")}, new List<AclGroup> {new AclGroup() {Id = 1}} ));
            var readAcl = _aclBase.Acls.First(a => a.Action == ACLAction.Read);
            Assert.AreEqual(2, readAcl.Users.Count);
            Assert.AreEqual(2, readAcl.Groups.Count);
            _aclBase.RemoveAcl(new ACL(ACLAction.Read, new List<AclUser> { new AclUser("2") }, null));
            _aclBase.RemoveAcl(new ACL(ACLAction.Read, new List<AclUser> { new AclUser("3") }, null));
            _aclBase.RemoveAcl(new ACL(ACLAction.Read, new List<AclUser> { new AclUser("3") }, null));
            Assert.AreEqual(0, readAcl.Users.Count);
            Assert.IsTrue(_aclBase.Acl.Contains("\"Users\":[]"));
        }
    }
}
