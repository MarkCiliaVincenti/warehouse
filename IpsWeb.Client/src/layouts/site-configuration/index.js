import { useState } from "react";
import { Grid, Stack, Zoom } from "@mui/material";
import SuiBox from "components/SuiBox";
import Footer from "examples/Footer";
import DashboardLayout from "examples/LayoutContainers/DashboardLayout";
import DashboardNavbar from "examples/Navbars/DashboardNavbar";
import Gateways from "./components/gateways";
import Sites from "./components/sites";
import SetSite from "./components/set-site";
import SetGateway from "./components/set-gateway";
import * as auth from 'auth-provider';
import { client } from "utils/api-client";
import { useQuery } from "react-query";

const SiteConfiguration = () => {
  const [refresh, updateRefreshState] = useState();
  const forceUpdate = () => updateRefreshState(Date.now());

  const [searchTerm, setSearchTerm] = useState("");
  const onSearch = (value) => setSearchTerm(value);

  const [siteForEdit, setSiteForEdit] = useState(null);
  const [selectedSite, setSelectedSite] = useState(null);
  const onSelectItem = (item) => {
    resetGwToNull();
    if(siteForEdit){
      setSiteForEdit(item);
    }
    setSelectedSite(item)
  };
  const resetToNull = () => setSiteForEdit(null);
  const resetToDefault = () =>
    setSiteForEdit({
      id: "",
      name: "",
      topLength: 0,
      leftLength: 0,
      error: 0,
    });

  const [gwForEdit, setGwForEdit] = useState(null);
  const resetGwToNull = () => setGwForEdit(null);
  const resetGwToDefault = () =>
    setGwForEdit({
      macAddress: "",
      name: "",
      circumscribedRadius: 0,
      location: 0,
      envFactor: 0,
    });

    
  const fetchRegisteredGw = async () => {
    const token = await auth.getToken();
    const res = await client(`sites/gw-registered`, {token});
    return res.data;
  };
  const { data: gwRegistered } = useQuery(["gw-registered"], fetchRegisteredGw, {
    keepPreviousData: false,
    refetchOnWindowFocus: false,
  });
 
  return (
    <DashboardLayout>
      <DashboardNavbar onSearch={onSearch} />
      <SuiBox mb={3} py={3}>
        <Grid container spacing={3}>
          <Grid item xs={12} md={4}>
            <Grid container spacing={siteForEdit ? 3 : 0}>
              <Zoom in={Boolean(siteForEdit)}>
                <Grid item xs={12}>
                  {siteForEdit && (
                    <SetSite
                      item={siteForEdit}
                      onClose={resetToNull}
                      onSave={() => {
                        resetToNull();
                        forceUpdate();
                      }}
                    />
                  )}
                </Grid>
              </Zoom>
              <Grid item xs={12}>
                <Sites
                  onSelect={onSelectItem}
                  onEdit={() => {setSiteForEdit(selectedSite)}}
                  onAdd={resetToDefault}
                  onDelete={forceUpdate}
                  refresh={refresh}
                />
              </Grid>
            </Grid>
          </Grid>
          <Grid item xs={12} md={8}>
            <Grid container spacing={gwForEdit ? 3 : 0}>
              <Zoom in={Boolean(gwForEdit)}>
                <Grid item xs={12}>
                  {gwForEdit && (
                    <SetGateway
                      item={gwForEdit}
                      onClose={resetGwToNull}
                      onSave={() => {
                        resetGwToNull();
                        forceUpdate();
                      }}
                      gwRegistered={gwRegistered}
                    />
                  )}
                </Grid>
              </Zoom>
              <Grid item xs={12}>
                <Gateways
                 data={selectedSite}
                 onAdd={resetGwToDefault}
                 />
              </Grid>
            </Grid>
          </Grid>
        </Grid>
      </SuiBox>
      <Footer />
    </DashboardLayout>
  );
};

export default SiteConfiguration;