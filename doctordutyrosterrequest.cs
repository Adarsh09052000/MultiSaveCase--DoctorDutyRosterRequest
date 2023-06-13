 public async Task<HttpCustomResponseMessage> SaveDoctorRosterRequestDetails(DoctorRosterRequestDetails saveRosterRequestDetails)
        {
            try
            {

                await Task.Run(() => SaveDutyRosterRequestDetails(saveRosterRequestDetails));

                string primaryKey = saveRosterRequestDetails.DoctorRosterRequestHeader[0].RequestNo.ToString();

                HttpCustomResponseMessage response = new HttpCustomResponseMessage()
                {
                    Message = "Success",
                    HttpCode = 200,
                    PrimaryKey = primaryKey
                };

                return response;

            }

            catch (Exception ex)
            {
                String errMsg = ex.Message;

                if (errMsg.Contains("-20859") ==true)
                    { errMsg = "Roster already exists for the selected date range.  Please select a different date range"; }

                HttpCustomResponseMessage response = new HttpCustomResponseMessage()
                {
                    HttpCode = 400,
                    Message = errMsg
                };

                return response;
            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveRosterRequestDetails"></param>
        private void SaveDutyRosterRequestDetails(DoctorRosterRequestDetails saveRosterRequestDetails)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {

                    Database database = Database.Create("defaultConnectionString");

                    SaveDutyRosterRequestHeader(saveRosterRequestDetails.DoctorRosterRequestHeader[0], database);

                    SaveDutyRosterRequestDtl(saveRosterRequestDetails.DoctorRosterRequestDetail, database, saveRosterRequestDetails.DoctorRosterRequestHeader[0].RequestNo);

                    transactionScope.Complete();

                }

                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    throw ex;
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DutyDetailsHeaderData"></param>
        /// <param name="database"></param>
        private void SaveDutyRosterRequestHeader(DoctorRosterRequestHdr DutyDetailsHeaderData, Database database)
        {
            OracleCommand dbCommand = new OracleCommand();
            try
            {
                dbCommand = database.CreateStoreCommand("STP_INSERTUPDATEDUTYROSTERREQUEST");
                database.AddInputParameter(dbCommand, "pReqNo", OracleDbType.Decimal, DutyDetailsHeaderData.RequestNo);
                database.AddInputParameter(dbCommand, "pDoctorCode", OracleDbType.Varchar2, DutyDetailsHeaderData.DoctorCode);
                database.AddInputParameter(dbCommand, "pFromDate", OracleDbType.Date, DutyDetailsHeaderData.FromDate);
                database.AddInputParameter(dbCommand, "pToDate", OracleDbType.Date, DutyDetailsHeaderData.ToDate);
                database.AddInputParameter(dbCommand, "pMonth", OracleDbType.Varchar2, DutyDetailsHeaderData.Month);
                database.AddInputParameter(dbCommand, "pYear", OracleDbType.Varchar2, DutyDetailsHeaderData.Year);
                database.AddInputParameter(dbCommand, "pModifiedUser", OracleDbType.Varchar2, DutyDetailsHeaderData.ModifiedUser);
                database.AddInputParameter(dbCommand, "pStatus", OracleDbType.Varchar2, DutyDetailsHeaderData.Status);
                database.AddOutputParameter(dbCommand, "pRequestNo", OracleDbType.Decimal);

                using (OracleDataReader objReader = database.ExecuteReader(dbCommand))
                {

                    DutyDetailsHeaderData.RequestNo = decimal.Parse(dbCommand.Parameters["pRequestNo"].Value.ToString());

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbCommand != null)
                {
                    if (dbCommand.Connection != null)
                    {
                        dbCommand.Connection.Close();
                    }
                    dbCommand.Dispose();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="detailLists"></param>
        /// <param name="database"></param>
        /// <param name="detailNo"></param>
        private void SaveDutyRosterRequestDtl(List<DoctorRosterRequestDtl> detailLists, Database database, decimal RequestNo)
        {
            OracleCommand dbCommand = new OracleCommand();
            try
            {

                DataTable dtTable = new DataTable();
                dtTable = MedinousClass.ToDataTable(detailLists);

                dbCommand = database.CreateStoreCommand("STP_INSERTUPDATEDUTYROSTERREQDTLS");
                database.AddInputParameter(dbCommand, "pDetailNo", OracleDbType.Decimal, "DetailNo", DataRowVersion.Current);
                database.AddInputParameter(dbCommand, "pLocationCode", OracleDbType.Varchar2, "LocationCode", DataRowVersion.Current);
                database.AddInputParameter(dbCommand, "pClinicCode", OracleDbType.Varchar2, "ClinicCode", DataRowVersion.Current);
                database.AddInputParameter(dbCommand, "pSessionCode", OracleDbType.Varchar2, "SessionCode", DataRowVersion.Current);
                database.AddInputParameter(dbCommand, "pRequestNo", OracleDbType.Decimal, RequestNo);
                database.AddInputParameter(dbCommand, "pTimeFrom", OracleDbType.Varchar2, "TimeFrom", DataRowVersion.Current);
                database.AddInputParameter(dbCommand, "pTimeTo", OracleDbType.Varchar2, "TimeTo", DataRowVersion.Current);
                database.AddInputParameter(dbCommand, "pSlotPeriod", OracleDbType.Varchar2, "SlotPeriod", DataRowVersion.Current);
                database.AddInputParameter(dbCommand, "pDayCode", OracleDbType.Varchar2, "DayCode", DataRowVersion.Current);
                database.AddInputParameter(dbCommand, "pModifiedUser", OracleDbType.Varchar2, "ModifiedUser", DataRowVersion.Current);
                database.AddInputParameter(dbCommand, "pDeletedFlag", OracleDbType.Varchar2, "DeletedFlag", DataRowVersion.Current);
                database.UpdateDataTable<DataTable>(dtTable, dbCommand, dbCommand, null);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbCommand != null)
                {
                    if (dbCommand.Connection != null)
                    {
                        dbCommand.Connection.Close();
                    }
                    dbCommand.Dispose();
                }
            }
        }
