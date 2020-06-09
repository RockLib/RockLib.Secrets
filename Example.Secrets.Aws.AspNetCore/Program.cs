using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using RockLib.Secrets;

namespace Example.Secrets.Aws.AspNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var credentialsFile = new NetSDKCredentialsFile();

            //credentialsFile.TryGetProfile("default", out var profile);

            //profile.Options.AccessKey = "ASIASRHLYM45QDLNWQMM";
            //profile.Options.SecretKey = "V62p4N2YVMIoMPaI03N4coeiPUqHIlVzxJNBSl1w";
            //profile.Options.Token = "FwoGZXIvYXdzEBQaDMY86r7wFEx4zNcIMCKAAjzyzR2UviCqnEuszVsDyZVYJ+seTAXRS/2zFVMOLW+EHd90UIzYWq+1yqISzKPmuIem1rw5KjEAInMzDWG2bPb7mQO3DBMr+jeB/Yk88fPh4XoojXSm6/8onbiJcWbmyDGhXNHtaURoDnsiCRitGZCnifgtAxVVQVIXpCTlE320HF4RkuBPaUoikJQB45wDyiSPEw1rEwmBZt3kP0YsiZsDKr2l39eQqg6gwhD2HRn+zJrTAs5ZArO06KHgk82t7WQRSrzPwIG62lzTkfZ7h9HGiaRnaJ2lAeCphjY9oDtwmis7OPPFyqTyH7pFMK7Hw97MxKaHzrRDXAZJTF4q7jIo8KXc9gUyKxPVCguA37Srd0D3PuwvMT1aNeurGdqR3buo7sYhfxeMzSkau7Wp70SlvgI=";
            //credentialsFile.RegisterProfile(profile);

            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                    builder.SetSecretExceptionHandler(context => Console.WriteLine(context.Exception))
                        .AddRockLibSecrets())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
