<?php
$IDENT = $_GET["id"];
$page = shell_exec('dotnet.exe SOA.dll "'.$IDENT.'"');
echo "$page";
?>
