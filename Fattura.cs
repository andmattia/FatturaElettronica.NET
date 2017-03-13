﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FatturaElettronica.Impostazioni;
using FatturaElettronica.Common;
using System.Xml.Serialization;

[assembly: InternalsVisibleTo("Tests," +
    "PublicKey=00240000048000009400000006020000002400005253413100040000010001002bc3d9fc3ae589" +
    "2f31b57e84fbd4c108035bdac32363b22691795307395ad82e43f3da76e95f6851190228e6dac9" +
    "5aa160072ea70ee1a62a01e1d5e905dd7845ece57d28eb2d63b366073740af1a05bc216ca0e205" +
    "b7974ffb2b21289125bcffdaa556f95d7891c0167eef5473d1e016cdac83acaa1b4da9fe9a2b2c" +
    "bf5200bf")]

namespace FatturaElettronica
{
    public class Fattura : BaseClassSerializable
    {
        private readonly FatturaElettronicaHeader.Header _header;
        private readonly List<FatturaElettronicaBody.Body> _body;

        protected Fattura() {
            _header = new FatturaElettronicaHeader.Header();
            _body = new List<FatturaElettronicaBody.Body>();
        }

        public override void WriteXml(System.Xml.XmlWriter w)
        {
            w.WriteStartElement(RootElement.Prefix, RootElement.LocalName, RootElement.NameSpace);
            w.WriteAttributeString("versione", Header.DatiTrasmissione.FormatoTrasmissione);
            foreach (RootElement.XmlAttributeString a in RootElement.ExtraAttributes)
            {
                w.WriteAttributeString(a.Prefix, a.LocalName, a.ns, a.value);
            }
            base.WriteXml(w);
            w.WriteEndElement();
        }

        public override void ReadXml(System.Xml.XmlReader r) {
            r.MoveToContent();
            base.ReadXml(r);
        }

        public static Fattura CreateInstance(Instance formato)
        {
            var f = new Fattura();

            switch (formato)
            {
                case Instance.PubblicaAmministrazione:
                    f.Header.DatiTrasmissione.FormatoTrasmissione = FormatoTrasmissione.PubblicaAmministrazione;
                    break;
                case Instance.Privati:
                    f.Header.DatiTrasmissione.FormatoTrasmissione = FormatoTrasmissione.Privati;
                    f.Header.DatiTrasmissione.CodiceDestinatario = new string('0', 7);
                    break;
            }

            return f;
        }

        /// IMPORTANT
        /// Each data property must be flagged with the Order attribute or it will be ignored.
        /// Also, properties must be listed with the precise order in the specification.

        /// <summary>
        /// Intestazione della comunicazione.
        /// </summary>
        [DataProperty][XmlElement(ElementName="FatturaElettronicaHeader")]
        public FatturaElettronicaHeader.Header Header { 
            get { return _header; }
        }

        /// <summary>
        /// Lotto di fatture incluse nella comunicazione.
        /// </summary>
        /// <remarks>Il blocco ha molteciplità 1 nel caso di fattura singola; nel caso di lotto di fatture, si ripete
        /// per ogni fattura componente il lotto stesso.</remarks>
        [DataProperty][XmlElement(ElementName="FatturaElettronicaBody")]
        public List<FatturaElettronicaBody.Body> Body {
            get { return _body; }
        }

    }
}