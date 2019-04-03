using RockLib.Configuration.ObjectFactory;
using RockLib.Secrets;
using System.Collections.Generic;

[assembly: ConfigSection("RockLib.Secrets", typeof(List<ISecretsProvider>))]
