namespace Saorsa.ACL.Model
{
    using System.ComponentModel.DataAnnotations;

    public class AclGroup
    {
        [Key]
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is AclGroup)
                return Id == ((AclGroup)obj).Id;
            return base.Equals(obj);
        }
    }
}
