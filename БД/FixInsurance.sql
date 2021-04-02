Select ins.* From insurance
Left Join insurance ins on insurance.name = ins.name
Where insurance.id_insurance != ins.id_insurance

Merge Into patient 
	using (
		Select insurance.id_insurance, Min(ins.id_insurance) minId
		From insurance
		Left Join insurance ins on insurance.name = ins.name
		Group By insurance.id_insurance
	) insurance
	on patient.id_insurance = insurance.id_insurance
WHEN MATCHED THEN
   UPDATE SET patient.id_insurance = insurance.minId;

Select * From insurance
Left Join patient on insurance.id_insurance = patient.id_insurance
Where patient.id_patient is null

Delete insurance From insurance
Left Join patient on insurance.id_insurance = patient.id_insurance
Where patient.id_patient is null


--были пациенты с привязанными страховыми компаниями, но из-за кривого импорта я продублировал все страховые на каждого пациента 
--то есть мне надо было взять пациента и поставить ему страховую с таким же именем, но наименьшим айдишниом


