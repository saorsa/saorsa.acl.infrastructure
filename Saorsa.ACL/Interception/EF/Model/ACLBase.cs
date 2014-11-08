namespace Saorsa.ACL.Interception.EF.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Newtonsoft.Json;
    using Saorsa.ACL.Interception.Helpers;
    using Saorsa.ACL.Model;

    public class AclBase
    {
        public AclBase()
        {
            Visible = true;
        }
        [Key]
        public long Id { get; set; }
        private List<ACL> _acls;
        public string Acl { get; set; }
        public bool Visible { get; set; }
        #region Basic Audit Properties
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        #endregion
        [NotMapped]
        public List<ACL> Acls
        {
            get
            {
                if(string.IsNullOrEmpty(Acl))
                    _acls = new List<ACL>();
                
                return this._acls ?? (this._acls = JsonConvert.DeserializeObject<List<ACL>>(Acl));

            }
            set
            {
                var aclCleanedUpValue = CleanUpAcl(value);
                this.Acl = JsonConvert.SerializeObject(aclCleanedUpValue);
                this._acls = aclCleanedUpValue;
            }
        }


        /// <summary>
        /// Groups all action permissions into one acl (if any)
        /// </summary>
        /// <param name="acls"></param>
        /// <returns></returns>
        public static List<ACL> CleanUpAcl(IReadOnlyCollection<ACL> acls)
        {
            var acl = new List<ACL>();
            //In case there are any repeating actions - group all information in one
            if (acls.Count(a => a.Action == ACLAction.Read) > 1)
            {
                var users = acls.Where(a => a.Action == ACLAction.Read).SelectMany(a => a.U)
                                .Distinct(new AclUsersComparer()).ToList();
                var groups = acls.Where(a => a.Action == ACLAction.Read).SelectMany(a => a.G)
                                .Distinct(new AclGroupComparer()).ToList();
                acl.Add(new ACL(ACLAction.Read,users,groups));
            }
            else
            {
                var readPermission = acls.FirstOrDefault(a => a.Action == ACLAction.Read);
                if(readPermission!=null)
                    acl.Add(readPermission);
            }
            if (acls.Count(a => a.Action == ACLAction.Write) > 1)
            {
                var users = acls.Where(a => a.Action == ACLAction.Write).SelectMany(a => a.U)
                                .Distinct(new AclUsersComparer()).ToList();
                var groups = acls.Where(a => a.Action == ACLAction.Write).SelectMany(a => a.G)
                                .Distinct(new AclGroupComparer()).ToList();
                acl.Add(new ACL(ACLAction.Write, users, groups));
            }
            else
            {
                var writePermission = acls.FirstOrDefault(a => a.Action == ACLAction.Write);
                if (writePermission != null)
                    acl.Add(writePermission);
            }
            return acl;
        }

        public void AddAcl(ACL acl)
        {
            //Todo when testing check if the acl is also applied to the string value

            if (this.Acls.Any(a=>a.Action==acl.Action))
            {
                var existingAcl = this.Acls.First(a => a.Action == acl.Action);
                //Add the non existing users to the acl
                if (acl.U != null && acl.U.Count > 0)
                    existingAcl.U.AddRange(acl.U.Where(a => !existingAcl.U.Any(au => au.Equals(a))));
                //Add the non existing groups to the acl
                if (acl.G != null && acl.G.Count > 0)
                    existingAcl.G.AddRange(acl.G.Where(a => !existingAcl.G.Any(au => au.Equals(a))));
            }
            else
            {
                //Ensure that there are no duplicating items
                acl.U = acl.U.Distinct(new AclUsersComparer()).ToList();
                acl.G = acl.G.Distinct(new AclGroupComparer()).ToList();
                this.Acls.Add(acl);
            }
            //Trigger setter cleanup
            this.Acls=new List<ACL>(this.Acls);

        }
        public void RemoveAcl(ACL acl)
        {
            //Todo when testing check if the acl is also applied to the string value
            var existingAcl = this.Acls.FirstOrDefault(a => a.Action == acl.Action);
            if (existingAcl != null)
            {
                //Add the non existing users to the acl
                if (acl.U != null && acl.U.Count > 0)
                    existingAcl.U.RemoveAll(a=>acl.U.Any(au=>au.Id==a.Id));
                //Add the non existing groups to the acl
                if (acl.G != null && acl.G.Count > 0)
                    existingAcl.G.RemoveAll(a => acl.G.Any(au => au.Id == a.Id));
            }
            //Trigger setter cleanup
            this.Acls = new List<ACL>(this.Acls);
        }
    }
}
