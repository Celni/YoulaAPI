using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace YoulaApi.Models
{
    public class VerifyUriRequest : AuthRequestUriAbstract
    {
        private string Capabilities = "call_number_fragment,call_session_hash,background_verify,sms_retriever";

        private string Checks = "sms,push";

        public string DeviceAccount = "mail@brazzers.com";

        public string DeviceId = Guid.NewGuid().ToString();

        public string DeviceName = "VS+Emulator+5-inch+KitKat+(4.4)+XXHDPI+Phone";

        private double LocationAccuracy = 0.00;

        public double LocationLat = 47.640068;

        public double LocationLon = -122.129858;

        private string Mode = "manual";

        public string Phone { get; }

        private string PushToken = "ePZXxAytu4c%3AAPA91bEklK9EYLiaKY3AdXp3xECwmi5N5WgkgcWYAIFex6s8TvXBbDjwBjI6DGjHCxpmTg9nD3aGI-GS2RvpVZEoAmGiitT2tis8yKVn3R18Zc-D0WMYB59NJm_1ePqOYBEVMR7lAqrV";

        private string ServerKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA%2FAT8Vk4gYXZU7gYjlamqdikCK%2F0KO9YuXjHM7Yfi5Ou9pExYS7YfCPuSbB%2FAehJVqs%2BaZSTja7FRRU5fKwIvoqEav%2BwnUvGqaLggE8YxZNPTFA9kPAAwLlQj6vFEfEyjkPTyP8KHaJ2B%2FjLL24NMLczVqKlURLVl9Y2HMeLufDfmHpLajkgA1o9dZDpxXlAu0W0NLEWl3GLDVYvW9yo4cAZu8AVY5GtKS3V6pBRKso3yJXUdp%2B%2BHy7gYFO9amTuqiU4zZ15zes%2FUu2zfnbzBtoLZoD3lyVHjP0qflLDfXAnr7kvmv0sOS26PIxktwadqixs3MZY2eQgmWys5KLIN5QIDAQAB";

        public string ApplicationId = Guid.NewGuid().ToString();

        public string SessionId { get; }

        private string SimState = "unknown";

        public string SystemId = "fa3ed53c4af94a21";

        private int Version = Const.Version;


        public VerifyUriRequest(string phone)
        {
            Phone = phone;
            SessionId = CtrateMD5(null);
        }

        public override string CreateUri()
        {
            var strs = CompileParameters();
            var str = string.Join("&", strs);
            var uri = $"verify?{str}&signature={CreateSignature()}";
            return uri;
        }

        protected override string CreateSignature()
        {
            var str = string.Join("", CompileParameters()).Replace("=", "");
            return CtrateMD5($"verify{str}{this.Key}");
        }

        private List<string> CompileParameters()
        {
            var strs = new List<string>();
            strs.Add($"application={Uri.EscapeDataString(this.Application)}");
            strs.Add($"application_id={Uri.EscapeDataString(this.ApplicationId)}");
            strs.Add($"capabilities={Uri.EscapeDataString(this.Capabilities)}");
            strs.Add($"checks={Uri.EscapeDataString(this.Checks)}");
            strs.Add($"device_account={Uri.EscapeDataString(this.DeviceAccount)}");
            strs.Add($"device_id={Uri.EscapeDataString(this.DeviceId)}");
            strs.Add($"device_name={Uri.EscapeDataString(this.DeviceName)}");
            strs.Add($"language={Uri.EscapeDataString(this.Language)}");
            strs.Add($"location_accuracy={this.LocationAccuracy.ToString().Replace(',', '.')}");
            strs.Add($"location_lat={this.LocationLat.ToString().Replace(',', '.')}");
            strs.Add($"location_lon={this.LocationLon.ToString().Replace(',', '.')}");
            strs.Add($"mode={Uri.EscapeDataString(this.Mode)}");
            strs.Add($"phone={Uri.EscapeDataString(this.Phone)}");
            strs.Add($"platform={Uri.EscapeDataString(this.Platform)}");
            strs.Add($"push_token={Uri.EscapeDataString(this.PushToken)}");
            strs.Add($"server_key={Uri.EscapeDataString(this.ServerKey)}");
            strs.Add($"service={Uri.EscapeDataString(this.Service)}");
            strs.Add($"session_id={Uri.EscapeDataString(this.SessionId)}");
            strs.Add($"sim_state={Uri.EscapeDataString(this.SimState)}");
            strs.Add($"system_id={Uri.EscapeDataString(this.SystemId)}");
            return strs;
        }

        public override string ToString()
        {
            return CreateUri();
        }
    }
}
