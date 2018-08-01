using System;
using System.Collections.Generic;
using System.Text;

namespace YoulaApi.Models
{
    public class AttemptUriRequest : AuthRequestUriAbstract
    {
        public string Code { get; }

        public string Phone { get; }

        private string CodeSource = "USER_INPUT";

        public string Id { get; }

        private string Internal = "verify";

        public string ApplicationId;

        public AttemptUriRequest(string phone, string code, string session_id)
        {
            Code = code;
            Phone = phone;
            Id = session_id;
        }

        public AttemptUriRequest(VerifyResponse response, string code)
        {
            Code = code;
            Phone = response.ModifiedPhoneNumber;
            Id = response.SessionId;
        }

        public override string CreateUri()
        {
            var strs = CreateParameters();
            var str = string.Join("&", strs);
            var uri = $"attempt?{str}&signature={CreateSignature()}";
            return uri;
        }

        protected override string CreateSignature()
        {
            var str = string.Join("", CreateParameters()).Replace("=", "");
            return CtrateMD5($"attempt{str}{this.Key}");
        }

        private List<string> CreateParameters()
        {
            var result = new List<string>();
            result.Add($"application={Uri.EscapeDataString(this.Application)}");
            result.Add($"application_id={Uri.EscapeDataString(this.ApplicationId)}");
            result.Add($"code={Uri.EscapeDataString(this.Code)}");
            result.Add($"code_source={Uri.EscapeDataString(this.CodeSource)}");
            result.Add($"id={Uri.EscapeDataString(this.Id)}");
            result.Add($"internal={Uri.EscapeDataString(this.Internal)}");
            result.Add($"language={Uri.EscapeDataString(this.Language)}");
            result.Add($"phone={Uri.EscapeDataString(this.Phone)}");
            result.Add($"platform={Uri.EscapeDataString(this.Platform)}");
            result.Add($"service={Uri.EscapeDataString(this.Service)}");
            return result;
        }

        public override string ToString()
        {
            return this.CreateUri();
        }
    }
}
