﻿namespace FatturaElettronica.Validators
{
    public class RegimeFiscaleDomainValidator<T> : DomainValidator<T>
    {
        protected override string[] Domain
        {
            get
            {
                return Tabelle.RegimeFiscale.Codici;
            }
        }
    }
}
