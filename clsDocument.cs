﻿using System;
namespace ResilienceClasses
{
    public class clsDocument
    {
        public enum Type
        {
            Mortgage, PurchaseStatement, EscrowInstructionLetter, ClosingProtectionLetter, TitleCommitment,
            ProFormaLenderPolicy, RehabBid, HomeownersInsurance, TitleWork, Discharge, Calculator, BPO, SaleContract, LoanPayoffLetter,
            SaleStatement, ProfitStatement
        }

        public static string strDocumentPath = "/Users/" + Environment.UserName + "/Documents/Professional/Resilience/tblDocument.csv";
        public static int NameColumn = 1;
        public static int PropertyColumn = 2;
        public static int TypeColumn = 3;
        private int iDocumentID;
        private string strName;
        private int iPropertyID;
        private clsDocument.Type tType;

        public clsDocument(int id)
        {
            this._Load(id);
        }

        public clsDocument(string name, int propertyID, clsDocument.Type type)
        {
            this.iDocumentID = _NewDocumentID();
            this.strName = name;
            this.iPropertyID = propertyID;
            this.tType = type;
        }

        public string Name() { return this.strName; }
        public int PropertyID() { return this.iPropertyID; }
        public string PropertyAddress() { return new clsProperty(this.iPropertyID).Address(); }
        public clsDocument.Type DocumentType() { return this.tType; }

        public bool Save()
        {
            return this.Save(clsDocument.strDocumentPath);
        }

        public bool Save(string path)
        {
            clsCSVTable tbl = new clsCSVTable(path);
            if (this.iDocumentID == tbl.Length() + 1)
            {
                string[] strValues = new string[tbl.Width() - 1];
                strValues[clsDocument.NameColumn - 1] = this.strName;
                strValues[clsDocument.PropertyColumn - 1] = this.iPropertyID.ToString();
                strValues[clsDocument.TypeColumn - 1] = ((int)this.tType).ToString();
                tbl.New(strValues);
                return tbl.Save();
            }
            else
            {
                if ((this.iDocumentID <= tbl.Length()) && (this.iDocumentID > 0))
                {
                    if (
                        tbl.Update(this.iDocumentID, clsDocument.NameColumn, this.strName) &&
                        tbl.Update(this.iDocumentID, clsDocument.PropertyColumn, this.iPropertyID.ToString()) &&
                        tbl.Update(this.iDocumentID, clsDocument.TypeColumn, ((int)this.tType).ToString()))
                    {
                        return tbl.Save();
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        private int _NewDocumentID()
        {
            clsCSVTable tbl = new clsCSVTable(clsDocument.strDocumentPath);
            return tbl.Length() + 1;
        }

        private bool _Load(int id)
        {
            clsCSVTable tbl = new clsCSVTable(clsDocument.strDocumentPath);
            if (id < tbl.Length())
            {
                this.iDocumentID = id;
                this.strName = tbl.Value(id, clsDocument.NameColumn);
                this.iPropertyID = Int32.Parse(tbl.Value(id, clsDocument.PropertyColumn));
                this.tType = (clsDocument.Type)Int32.Parse(tbl.Value(id, clsDocument.TypeColumn));
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}