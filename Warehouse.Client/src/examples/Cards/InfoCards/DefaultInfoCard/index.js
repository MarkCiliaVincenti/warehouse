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

// prop-types is library for typechecking of props
import PropTypes from "prop-types";

// @mui material components
import Card from "@mui/material/Card";
import Divider from "@mui/material/Divider";
import Icon from "@mui/material/Icon";

// Soft UI Dashboard React components
import SuiBox from "components/SuiBox";
import SuiTypography from "components/SuiTypography";

import { ScaleLoader } from "react-spinners";

function DefaultInfoCard({ color, icon, title, description, value, isLoading }) {
  return (
    <Card>
      <SuiBox p={2} mx={3} display="flex" justifyContent="center">
        <SuiBox
          display="grid"
          justifyContent="center"
          alignItems="center"
          bgColor={color}
          color="white"
          width="4rem"
          height="4rem"
          shadow="md"
          borderRadius="lg"
          variant="gradient"
        >
          <Icon fontSize="default">{icon}</Icon>
        </SuiBox>
      </SuiBox>
      <SuiBox pb={2} px={2} textAlign="center" lineHeight={1.25}>
        <SuiTypography variant="h6" fontWeight="medium" textTransform="capitalize">
          {title}
        </SuiTypography>
        {description && (
          <SuiTypography variant="caption" color="text" fontWeight="regular">
            {description}
          </SuiTypography>
        )}
        {description && !value ? null : <Divider />}
        {value && (
          <SuiTypography variant="h5" fontWeight="medium">
            {value}
          </SuiTypography>
        )}
        <ScaleLoader
          loading={isLoading}
          color={"#17c1e8"}
          cssOverride={{ position: "absolute", display: "inherit", top: "78%", left: "42%" }}
        />
      </SuiBox>
    </Card>
  );
}

// Setting default values for the props of DefaultInfoCard
DefaultInfoCard.defaultProps = {
  color: "info",
  value: "",
  description: "",
  isLoading: false,
};

// Typechecking props for the DefaultInfoCard
DefaultInfoCard.propTypes = {
  color: PropTypes.oneOf(["primary", "secondary", "info", "success", "warning", "error", "dark"]),
  icon: PropTypes.node.isRequired,
  title: PropTypes.string.isRequired,
  description: PropTypes.string,
  value: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
  isLoading: PropTypes.bool,
};

export default DefaultInfoCard;
