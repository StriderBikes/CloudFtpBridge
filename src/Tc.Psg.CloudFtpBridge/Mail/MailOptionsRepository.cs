﻿using LiteDB;
using Microsoft.Extensions.Options;

namespace Tc.Psg.CloudFtpBridge.Mail
{
    public class MailOptionsRepository : IMailOptionsRepository
    {
        public MailOptionsRepository(IOptions<CloudFtpBridgeOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public CloudFtpBridgeOptions Options { get; private set; }

        public MailOptions Get()
        {
            MailOptions options;

            using (LiteDatabase db = new LiteDatabase(Options.GetFullDatabaseFileName()))
            {
                LiteCollection<MailOptions> optionsCollection = db.GetCollection<MailOptions>();

                options = optionsCollection.FindOne(x => true) ?? MailOptions.Empty;
            }

            return options;
        }

        public void Set(MailOptions options)
        {
            using (LiteDatabase db = new LiteDatabase(Options.GetFullDatabaseFileName()))
            {
                LiteCollection<MailOptions> optionsCollection = db.GetCollection<MailOptions>();

                optionsCollection.Delete(x => true);
                optionsCollection.Insert(options);
            }
        }
    }
}
