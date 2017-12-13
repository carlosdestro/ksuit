<html>
<head>
    <title>TEMPLATE</title>

    <link rel="stylesheet" type="text/css" href="../view/style.css">

    <script src="../3rd/jquery-2.1.4.min.js"></script>

    <script src="../3rd/jquery-ui.min.js"></script>

	<script src="../3rd/datepicker-pt-BR.js"></script>

	<script src="../3rd/jquery.mask.min.js"></script>

    <link rel="stylesheet" type="text/css" href="../3rd/jquery-ui.min.css">

    <link rel="stylesheet" type="text/css" href="../3rd/jquery-ui.theme.min.css">

   
</head>
<body>
	<div id="menu">
		
	</div>
	<div class="content">
		<div class="logo"></div>
		
		<div class="login">
			<span class="title">Login</span>
			<br>
			<br>

			<form method="POST" action="logar.php">
				
				<div class="">
					<span class="caption">Usu&aacute;rio</span> <input name="usuario" id="usuario" >
				</div>

				<br>

				<div class="">
					<span class="caption">Senha</span> <input  type="password" name="senha" id="senha" >
				</div>

				<br>									
				
				<input type="submit" value="Entrar" class="button" id="login">
			</form>
			<span class="title count"></span>

		</div>
	</div>

</body>
</html>


