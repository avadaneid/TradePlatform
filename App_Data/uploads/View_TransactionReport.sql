USE [TradingPlatform]
GO

/****** Object:  View [dbo].[TransactionReport]    Script Date: 5/13/2020 6:35:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE view [dbo].[TransactionReport]
as
select NEWID() as Id,CompanyName as CompanyName,cast(CompanyIdentifier as int) CompanyIdentifier,cast(Date as date) Date,cast(OpenPrice as decimal) OpenPrice,cast(ClosingPrice as decimal) ClosingPrice,cast(Volume as int) Volume,cast(Value as decimal) Value,cast(High as decimal) High,cast(Low as decimal) Low from 

(select CompanyName CompanyName,min(Price) as Low,max(price) as High,sum(Quantity) as Volume,sum(Price*Quantity) as Value,dateadd(DAY,0, datediff(day,0, CreatedOn)) as Date,CompanyIdentifier
from Transactions
group by dateadd(DAY,0, datediff(day,0, CreatedOn)),CompanyIdentifier,CompanyName) as t1,

(SELECT * FROM
(select Price as OpenPrice,dateadd(DAY,0, datediff(day,0, CreatedOn)) as Date1,CompanyIdentifier as CompanyIdentifier1, 
ROW_NUMBER() over(partition by CompanyIdentifier,dateadd(DAY,0, datediff(day,0, CreatedOn)) order by CREATEDON ) AS 'COUN'
from Transactions 
) AS AL
where COUN = 1) as t2,

(SELECT * FROM
(select Price ClosingPrice,dateadd(DAY,0, datediff(day,0, CreatedOn)) as Date2,CompanyIdentifier as CompanyIdentifier2, 
ROW_NUMBER() over(partition by CompanyIdentifier,dateadd(DAY,0, datediff(day,0, CreatedOn)) order by CREATEDON desc) AS 'COUN'
from Transactions 
) AS AL
where COUN = 1) as t3

where t1.CompanyIdentifier = t2.CompanyIdentifier1 and t2.CompanyIdentifier1 = t3.CompanyIdentifier2
and t1.Date = t2.Date1 and t2.Date1 = t3.Date2



GO


