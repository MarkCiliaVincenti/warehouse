import { useEffect, useRef, useState } from "react";
import { Card, Grid, Icon, IconButton, Tooltip } from "@mui/material";
import SuiBox from "components/SuiBox";
import SuiTypography from "components/SuiTypography";
import DashboardLayout from "examples/LayoutContainers/DashboardLayout";
import DashboardNavbar from "examples/Navbars/DashboardNavbar";
import Footer from "examples/Footer";
import CanvasSite from "./conponents/canvas-site";
import { setProduct, setSite, useStoreController, setBeacon } from "../../context/store.context";
import { useQuery } from "react-query";
import { fetchSiteById } from "../../utils/query-keys";
import { getSiteById } from "../../api/warehouse";
import CanvasList from "./conponents/canvas-list/index0";
import { useNavigate } from "react-router-dom";
import CanvasListByProduct from "./conponents/canvas-list/list-by-product";
import BeaconTelemetry from "./conponents/beacon-telemetry";
import BeaconTelemetryCharts from "./conponents/beacon-charts/indiex";
import Tabs from "@mui/material/Tabs";
import Tab from "@mui/material/Tab";
import TabOutlinedIcon from "@mui/icons-material/TabOutlined";
import SensorsSharpIcon from "@mui/icons-material/SensorsSharp";

const Warehouse = () => {
  const [selectedView, setSelectView] = useState(0);
  const handleChange = (event, value) => {
    setSelectView(value);
    if (value === 0) {
      setTimeout(() => {
        onBeaconSelect({ ...selectedBeacon });
      }, 100);
    }
  };
  const [controller, dispatch] = useStoreController();
  const cardRef = useRef();
  const [width, setWidth] = useState(0);
  const [height, setHeight] = useState(0);

  const { site: currentSite, product: currentProduct, beacon: selectedBeacon } = controller;
  const onBeaconSelect = (item) => setBeacon(dispatch, item);
  const onProductSelect = (item) => {
    onBeaconSelect(null);
    setProduct(dispatch, item);
  };
  const onSiteSelect = (item) => {
    onBeaconSelect(null);
    setSite(dispatch, item);
  };
  const resize = () => {
    if (!cardRef.current) return;
    setWidth(cardRef.current.offsetWidth);
    setHeight(cardRef.current.offsetHeight);
  };
  useEffect(() => {
    window.addEventListener("resize", resize);
    cardRef.current && resize();
    return () => {
      window.removeEventListener("resize", resize);
    };
  }, [cardRef]);
  const navigate = useNavigate();
  const { data: response, isSuccess } = useQuery(
    [fetchSiteById, currentSite],
    () => getSiteById(currentSite.id),
    {
      enabled: Boolean(currentSite),
    }
  );
  useEffect(() => {
    !Boolean(currentSite) && navigate("/home");
  }, []);
  return (
    <DashboardLayout>
      <DashboardNavbar />
      <SuiBox py={2}>
        <Grid container spacing={3}>
          <Grid item xs={12} md={4}>
            <Card>
              <SuiBox display="flex" justifyContent="space-between" alignItems="center" p={3}>
                <SuiTypography
                  variant="h6"
                  color={
                    currentSite && currentSite.products
                      ? "info"
                      : currentProduct && currentProduct.sites
                      ? "primary"
                      : "secondary"
                  }
                >
                  {currentSite && currentSite.products
                    ? currentSite.name
                    : currentProduct && currentProduct.sites
                    ? currentProduct.name
                    : "n/a"}
                </SuiTypography>
                <SuiBox display="flex" alignItems="center" mt={{ xs: -1, sm: 0 }}>
                  <IconButton size="xl" color="inherit" onClick={() => navigate("/home")}>
                    <Tooltip title="Back">
                      <Icon>reply</Icon>
                    </Tooltip>
                  </IconButton>
                </SuiBox>
              </SuiBox>
              <SuiBox px={2}>
                <Grid container spacing={3}>
                  <Grid item xs={12}>
                    {currentSite && currentSite.products && (
                      <CanvasList
                        selectedSite={currentSite}
                        selectedProduct={currentProduct}
                        onProductSelect={onProductSelect}
                        selectedBeacon={selectedBeacon}
                        onBeaconSelect={onBeaconSelect}
                      />
                    )}
                    {currentProduct && currentProduct.sites && (
                      <CanvasListByProduct
                        selectedSite={currentSite}
                        selectedProduct={currentProduct}
                        onSiteSelect={onSiteSelect}
                        selectedBeacon={selectedBeacon}
                        onBeaconSelect={onBeaconSelect}
                      />
                    )}
                  </Grid>
                </Grid>
              </SuiBox>
            </Card>
          </Grid>
          <Grid item xs={12} md={8}>
            <Grid container spacing={2}>
              <Grid item xs={12}>
                <Card>
                  <SuiBox p={3}>
                    <Tabs value={selectedView} onChange={handleChange} aria-label="navigation">
                      <Tab
                        icon={<TabOutlinedIcon />}
                        iconPosition="start"
                        label={currentSite ? currentSite.name : "n/a"}
                      />
                      {selectedBeacon && (
                        <Tab
                          icon={<SensorsSharpIcon />}
                          iconPosition="start"
                          label={selectedBeacon.name || selectedBeacon.macAddress}
                        />
                      )}
                    </Tabs>
                  </SuiBox>
                </Card>
              </Grid>
              <Grid item xs={12}>
                {selectedView == 0 && (
                  <Card>
                    <SuiBox
                      display="flex"
                      justifyContent="space-between"
                      alignItems="center"
                      py={2}
                      px={3}
                    >
                      <SuiTypography variant="h6" gutterBottom>
                        {currentSite ? currentSite.name : "n/a"}
                      </SuiTypography>
                    </SuiBox>
                    <SuiBox ref={cardRef} sx={{ width: "auto", height: "60vh" }}>
                      {cardRef.current && isSuccess && (
                        <CanvasSite
                          width={width}
                          height={height}
                          site={response}
                          beacon={selectedBeacon}
                        />
                      )}
                    </SuiBox>
                  </Card>
                )}
                {selectedView == 1 && Boolean(selectedBeacon) && (
                  <Grid container spacing={3}>
                    <Grid item xs={12} md={6}>
                      <BeaconTelemetry item={selectedBeacon} />
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <BeaconTelemetryCharts item={selectedBeacon} />
                    </Grid>
                  </Grid>
                )}
              </Grid>
            </Grid>
          </Grid>
        </Grid>
      </SuiBox>
      <Footer />
    </DashboardLayout>
  );
};

export default Warehouse;
