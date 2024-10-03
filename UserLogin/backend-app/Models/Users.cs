using Org.BouncyCastle.Asn1.Crmf;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace backend_app.Models {
    public class Users {
        public int Id {
            get; set;
        }
        public string? Name {
            get; set;
        }

        [Required(ErrorMessage = "メールアドレスは必須です")]
        [EmailAddress(ErrorMessage = "無効なメールアドレスです")]
        [StringLength(255, ErrorMessage = "メールアドレスは255文字以内でなければなりません")]
        public string? Email {
            get; set;
        }

        [Required(ErrorMessage = "パッケージは必須です")]
        [StringLength(255, ErrorMessage = "パスワードは255文字以内でなければなりません")]
        public string? PasswordHash {
            get; set;
        }

        public DateTime CreatedAt {
            get; set;
        } = DateTime.UtcNow;
    }
}
