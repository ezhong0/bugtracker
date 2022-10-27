<?php
$username = $_POST['username'];
$password = $_POST['password'];

if(!empty($username) || !empty($password)) {
    $host = "localhost";
    $dbUsername = "root";
    $dbPassword = "Bl@ckpink";
    $dbname = "bugtrackerdb";

    $conn = new mysqli($host, $dbUsername, $dbPassword, $dbname);

    if(mysqli_connect_error()) {
        die('Connect Error('. mysqli_connect_errno().')'. mysqli_connect_error());
    } else {
        $SELECT = "select USERNAME from USER where USERNAME = ? and PASSWORD = ? Limit 1";
        $stmt = $conn->prepare($SELECT);
        $stmt->bind_param("ss", $username, $password);
        $stmt->execute();
        $stmt->bind_result($username);
        $stmt->store_result();
        $rnum=$stmt->num_rows(); 
        if ($rnum==0) {
            echo "fail password";
        } else {
            echo "successful login";
        }
        $stmt->close();
        $conn->close();
    }
}
?>