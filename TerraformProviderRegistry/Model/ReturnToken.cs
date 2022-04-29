namespace TerraformProviderRegistry.Model
{
    public class ReturnToken
    {

        public ReturnToken()
        {
            this.access_token = string.Empty;
        }

        public ReturnToken(string access_token)
        {
            this.access_token = access_token;
        }


        public string access_token { get; set; }

    }

}
