# Baby-NI
Creating the main table:
------------------------
CREATE TABLE TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER (
NETWORK_SID VARCHAR,
DATETIME_KEY DATETIME,
NEID FLOAT,
OBJECT_ VARCHAR, 
TIME_ DATETIME,
INTERVAL_ INT,
DIRECTION VARCHAR,
NEALIAS VARCHAR,
NETYPE VARCHAR,
RXLEVELBELOWTS1 FLOAT,
RXLEVELBELOWTS2 FLOAT,
MINREXLEVEL FLOAT,
MAXRXLEVEL FLOAT,
TXLEVELABOVETS1 FLOAT,
MINTXLEVEL FLOAT,
MAXTLEVEL FLOAT,
FAILUREDESCRIPTION VARCHAR,
LINK VARCHAR,
TID VARCHAR,
FARENDTID VARCHAR,
SLOT INT,
PORT INT,
FILE_NAME VARCHAR
);
------------------------
Creating the aggregated tables
------------------------
Daily and hourly
------------------------
CREATE TABLE TRANS_MW_AGG_SLOT_HOURLY (
DATETIME_KEY DATETIME,
NETYPE VARCHAR,
NEALIAS VARCHAR,
MAX_RX_LEVEL FLOAT,
MAX_TX_LEVEL FLOAT,
RSL_DEVIATION FLOAT,
FILE_NAME VARCHAR
);
------------------------
CREATE TABLE TRANS_MW_AGG_SLOT_DAILY (
DATETIME_KEY DATETIME,
NETYPE VARCHAR,
NEALIAS VARCHAR,
MAX_RX_LEVEL FLOAT,
MAX_TX_LEVEL FLOAT,
RSL_DEVIATION FLOAT,
FILE_NAME VARCHAR
);
------------------------
LOGGER FILE
------------------------
CREATE TABLE LOGGER (
FILE_NAME VARCHAR,
ACTION_NAME VARCHAR,
ACTION_DATE TIMESTAMP
);
