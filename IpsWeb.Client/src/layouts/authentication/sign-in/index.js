/**
=========================================================
* Soft UI Dashboard React - v3.1.0
=========================================================

* Product Page: https://www.creative-tim.com/product/soft-ui-dashboard-react
* Copyright 2022 Creative Tim (https://www.creative-tim.com)

Coded by www.creative-tim.com

 =========================================================

* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
*/

import { useState } from "react";

// react-router-dom components
import { Link } from "react-router-dom";

// @mui material components
import Switch from "@mui/material/Switch";

// Soft UI Dashboard React components
import SuiBox from "components/SuiBox";
import SuiTypography from "components/SuiTypography";
import SuiInput from "components/SuiInput";
import SuiButton from "components/SuiButton";

// Authentication layout components
import CoverLayout from "layouts/authentication/components/CoverLayout";

// Images
import curved9 from "assets/images/curved-images/curved-6.jpg";

import { useAuth } from "../../../context/auth.context";
import { useNavigate, Navigate } from "react-router-dom";

function SignIn() {
  const navigate = useNavigate();
  const { user, login: signIn } = useAuth();

  const [rememberMe, setRememberMe] = useState(true);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(undefined);
  const [buttonText, setButtonText] = useState("Sign in");
  const handleSetRememberMe = () => setRememberMe(!rememberMe);

  //----------------------------------------------------------
  const login = async (event) => {
    if (event) {
      event.preventDefault();
    }

    if (user && user.token) {
      return navigate("/home");
    }
    if (email === "") {
      return setError("You must enter your email.");
    }
    if (password === "") {
      return setError("You must enter your password");
    }

    setButtonText("Signing in");
    try {
      await signIn({ email, password });
      return navigate("/home");
    } catch (err) {
      if(err.error){
        return setError(err.error);
      } else if (err.errors) {
        let result = "";
        for (var key of Object.keys(err.errors)) {
          result += key + ": " + err.errors[key] + "\r\n";
        }
        return setError(result);
      }
      return setError(`Error occured: ${err.title}`);
    }
  };

  //----------------------------------------------------------

  return (
    <CoverLayout
      title="Welcome back"
      description={`${user && user.token ? "" : "Enter your email and password to sign in"}`}
      image={curved9}
    >
      {user && user.token ? (
        <div>
          <h3 style={{ textAlign: "center" }}>You are already signed in.</h3>
          <SuiBox mt={4} mb={1}>
            <SuiButton variant="gradient" color="info" fullWidth onClick={login}>
              {`Let's go`}
            </SuiButton>
          </SuiBox>
        </div>
      ) : (
        <SuiBox component="form" role="form">
          <SuiBox mb={2}>
            <SuiBox mb={1} ml={0.5}>
              <SuiTypography component="label" variant="caption" fontWeight="bold">
                Email
              </SuiTypography>
            </SuiBox>
            <SuiInput
              defaultValue={email}
              onChange={(event) => {
                setEmail(event.target.value);
                setError(undefined);
              }}
              type="email"
              placeholder="Email"
            />
          </SuiBox>
          <SuiBox mb={2}>
            <SuiBox mb={1} ml={0.5}>
              <SuiTypography component="label" variant="caption" fontWeight="bold">
                Password
              </SuiTypography>
            </SuiBox>
            <SuiInput
              defaultValue={password}
              onChange={(event) => {
                setPassword(event.target.value);
                setError(undefined);
              }}
              type="password"
              placeholder="Password"
            />
          </SuiBox>
          <SuiBox display="flex" alignItems="center">
            <Switch checked={rememberMe} onChange={handleSetRememberMe} />
            <SuiTypography
              variant="button"
              fontWeight="regular"
              onClick={handleSetRememberMe}
              sx={{ cursor: "pointer", userSelect: "none" }}
            >
              &nbsp;&nbsp;Remember me
            </SuiTypography>
          </SuiBox>
          <SuiBox mt={2} mb={2} textAlign="center">
            <h6
              style={{
                fontSize: ".8em",
                color: "red",
                textAlign: "center",
                fontWeight: 400,
                transition: ".2s all",
              }}
            >
              {error}
            </h6>
          </SuiBox>
          <SuiBox mt={4} mb={1}>
            <SuiButton variant="gradient" color="info" fullWidth onClick={login}>
              {buttonText}
            </SuiButton>
          </SuiBox>
          <SuiBox mt={3} textAlign="center">
            <SuiTypography variant="button" color="text" fontWeight="regular">
              Don&apos;t have an account?{" "}
              <SuiTypography
                component={Link}
                to="/authentication/sign-up"
                variant="button"
                color="info"
                fontWeight="medium"
                textGradient
              >
                Sign up
              </SuiTypography>
            </SuiTypography>
          </SuiBox>
        </SuiBox>
      )}
    </CoverLayout>
  );
}

export default SignIn;