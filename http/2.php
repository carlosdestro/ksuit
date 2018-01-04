<?php

function connect()
{
	try
	{
		$hostname = ".\\SQLEXPRESS";
		$port = 1433;
		$dbname = "Sis";
		$username = "sis";
		$pw = 'sis';

		$dbh = odbc_connect("Driver={ODBC Driver 13 for SQL Server};Server=$hostname;Database=$dbname;",$username,$pw);

		return $dbh;
	}	
	catch (Exception $e)
	{
		echo gethostname();
		
		echo "Failed to get DB handle: " . $e->getMessage() . "\n";

		exit;
	}
}

$hostname = ".\\SQLEXPRESS";
		$port = 1433;
		$dbname = "Sis";
		$username = "sis";
		$pw = 'sis';

			$dbh = connect();

//$stmt = odbc_exec ($dbh, "SET NOCOUNT ON;");
echo 1;
//		echo odbc_errormsg ($dbh);


		$stmt = odbc_exec ($dbh, "select * into #t from formulario;");
echo 1;

		$stmt = odbc_prepare ($dbh, "select * from #t;");

		$res = odbc_execute ($stmt, array(1));


		//return $stmt->fetchAll();
		while($r= odbc_fetch_array($stmt)){
			$result[]= $r;
		}


		print_r($result);

		$stmt = odbc_exec ($dbh, "select * from #t where 1="."1");

		//$res = odbc_execute ($stmt, array(1));


		echo odbc_errormsg ($dbh);

		$result = [];


		//return $stmt->fetchAll();
		while($r= odbc_fetch_array($stmt)){
			$result[]= $r;
		}

		print_r($result);





?>