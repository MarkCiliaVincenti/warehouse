import { useState } from "react";

// @mui material components
import Grid from "@mui/material/Grid";
import Icon from "@mui/material/Icon";

// Soft UI Dashboard React components
import SuiBox from "components/SuiBox";
import SuiTypography from "components/SuiTypography";

// Soft UI Dashboard React examples
import DashboardLayout from "examples/LayoutContainers/DashboardLayout";
import DashboardNavbar from "examples/Navbars/DashboardNavbar";
import Footer from "examples/Footer";

// Dashboard layout components
import Assets from "layouts/home/components/Assets";
import PositionEvents from "layouts/home/components/position-events";
import BuildByDevelopers from "./components/BuildByDevelopers";
import GradientLineChart from "examples/Charts/LineCharts/GradientLineChart";
import ReportsBarChart from "examples/Charts/BarCharts/ReportsBarChart";

import reportsBarChartData from "layouts/dashboard/data/reportsBarChartData";
import gradientLineChartData from "layouts/dashboard/data/gradientLineChartData";

// Soft UI Dashboard React base styles
import typography from "assets/theme/base/typography";

function Dashboard() {
  const [searchTerm, setSearchTerm] = useState("");
  const onSearch = (value) => setSearchTerm(value);

  const { size } = typography;
  const { chart, items } = reportsBarChartData;

  return (
    <DashboardLayout>
      <DashboardNavbar onSearch={onSearch} />
      <SuiBox py={3}>
        <Grid container spacing={3}>
          <Grid item xs={12} md={6} lg={4}>
            <PositionEvents searchTerm={searchTerm} />
          </Grid>
          <Grid item xs={12} md={6} lg={8}>
          <SuiBox mb={3}>
              <Assets searchTerm={searchTerm}/>
            </SuiBox>
            <SuiBox mb={3}>
              <BuildByDevelopers />
            </SuiBox>
            <SuiBox mb={3}>
              <Grid container spacing={3}>
                <Grid item xs={12} lg={5}>
                  <ReportsBarChart
                    title="active users"
                    description={
                      <>
                        (<strong>+23%</strong>) than last week
                      </>
                    }
                    chart={chart}
                    items={items}
                  />
                </Grid>
                <Grid item xs={12} lg={7}>
                  <GradientLineChart
                    title="Sales Overview"
                    description={
                      <SuiBox display="flex" alignItems="center">
                        <SuiBox fontSize={size.lg} color="success" mb={0.3} mr={0.5} lineHeight={0}>
                          <Icon className="font-bold">arrow_upward</Icon>
                        </SuiBox>
                        <SuiTypography variant="button" color="text" fontWeight="medium">
                          4% more{" "}
                          <SuiTypography variant="button" color="text" fontWeight="regular">
                            in 2021
                          </SuiTypography>
                        </SuiTypography>
                      </SuiBox>
                    }
                    height="20.25rem"
                    chart={gradientLineChartData}
                  />
                </Grid>
              </Grid>
            </SuiBox>
          </Grid>
        </Grid>
      </SuiBox>
      <Footer />
    </DashboardLayout>
  );
}

export default Dashboard;
