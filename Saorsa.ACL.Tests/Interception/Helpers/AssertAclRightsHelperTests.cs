using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Saorsa.ACL.Tests.Interception.Helpers
{
    using System.Collections.Generic;
    using Saorsa.ACL.Interception.EF.Model;
    using Saorsa.ACL.Interception.Helpers;
    using Saorsa.ACL.Model;
    using Saorsa.ACL.Tests.Interception.EF;

    [TestClass]
    public class AssertAclRightsHelperTests
    {
        private AclBase _aclBase;

        [TestInitialize]
        public void Initialize()
        {
            this._aclBase = new TestAclBaseImplementation
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
        public void Test_CanWrite()
        {
            Assert.IsTrue(AssertAclRightsHelper.CanWrite(_aclBase.Acl, "1", null));
            Assert.IsTrue(AssertAclRightsHelper.CanWrite(_aclBase.Acl, string.Empty, new List<AclGroup> { new AclGroup { Id = 1 } }));
            Assert.IsTrue(AssertAclRightsHelper.CanWrite(_aclBase.Acl, string.Empty, new List<AclGroup> { new AclGroup { Id = 1 }, new AclGroup { Id = 7 } }));
            Assert.IsFalse(AssertAclRightsHelper.CanWrite(_aclBase.Acl, "7", null));
            Assert.IsFalse(AssertAclRightsHelper.CanWrite(_aclBase.Acl, string.Empty, new List<AclGroup> { new AclGroup { Id = 3 } }));
            Assert.IsFalse(AssertAclRightsHelper.CanWrite(_aclBase.Acl, string.Empty, new List<AclGroup> { new AclGroup { Id = 3 }, new AclGroup { Id = 2 } }));
        }
        [TestMethod]
        public void Test_CanRead()
        {
            Assert.IsTrue(AssertAclRightsHelper.CanRead(_aclBase.Acl, "1", null));
            Assert.IsTrue(AssertAclRightsHelper.CanRead(_aclBase.Acl, "5", null));
            Assert.IsTrue(AssertAclRightsHelper.CanRead(_aclBase.Acl, string.Empty, new List<AclGroup> { new AclGroup { Id = 5 } }));
            Assert.IsTrue(AssertAclRightsHelper.CanRead(_aclBase.Acl, string.Empty, new List<AclGroup> { new AclGroup { Id = 3 } }));
            Assert.IsTrue(AssertAclRightsHelper.CanRead(_aclBase.Acl, string.Empty, new List<AclGroup> { new AclGroup { Id = 3 }, new AclGroup { Id = 7 } }));
            Assert.IsFalse(AssertAclRightsHelper.CanRead(_aclBase.Acl, "7", null));
            Assert.IsFalse(AssertAclRightsHelper.CanRead(_aclBase.Acl, string.Empty, new List<AclGroup> { new AclGroup { Id = 7 } }));
            Assert.IsFalse(AssertAclRightsHelper.CanRead(_aclBase.Acl, string.Empty, new List<AclGroup> { new AclGroup { Id = 7 }, new AclGroup { Id = 8 } }));
        }
    }
}
