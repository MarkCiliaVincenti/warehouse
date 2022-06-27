import SuiBox from "components/SuiBox";
import SuiTypography from "components/SuiTypography";
import SuiButton from "components/SuiButton";
import { Button, ButtonGroup, Card, Icon } from "@mui/material";
import Table from "examples/Tables/Table";

// Images
import routerIcon from "assets/images/internet-router.png";
import SuiAvatar from "components/SuiAvatar";

const Locations = [
  "Unknown",
  "Center",
  "TopCenter",
  "TopLeft",
  "TopRight",
  "BottomCenter",
  "BottomLeft",
  "BottomRight",
  "CenterLeft",
  "CenterRight",
];

function Gauge({ data }) {
  return (
    <SuiBox display="flex" alignItems="baseline" flexDirection="column">
      <SuiTypography variant="caption" fontWeight="medium">
        {data.mac}
      </SuiTypography>
      <SuiTypography variant="caption" color="secondary">
        <SuiTypography variant="caption" color="success">
          Radius:
        </SuiTypography>
        &nbsp;&nbsp;{data.radius}
        dbm
      </SuiTypography>
      <SuiTypography variant="caption" color="secondary">
        <SuiTypography variant="caption" color="success">
          TX power:
        </SuiTypography>
        &nbsp;&nbsp;{data.txPower}dbm
      </SuiTypography>
    </SuiBox>
  );
}

export default function Gateways({ data }) {
  return (
    <Card>
      <SuiBox display="flex" justifyContent="space-between" alignItems="center" p={3}>
        <SuiBox>
          <SuiTypography variant="h6">Gateways</SuiTypography>
        </SuiBox>
        {data && (
          <SuiBox display="flex" alignItems="center" mt={{ xs: 2, sm: 0 }} ml={{ xs: -1.5, sm: 0 }}>
          <SuiButton variant="gradient" color="primary" onClick={()=>{}}>
            <Icon sx={{ fontWeight: "bold" }}>add</Icon>
            &nbsp;new
          </SuiButton>
        </SuiBox>
        )}    
      </SuiBox>
      <SuiBox
        sx={{
          "& .MuiTableRow-root:not(:last-child)": {
            "& td": {
              borderBottom: ({ borders: { borderWidth, borderColor } }) =>
                `${borderWidth[1]} solid ${borderColor}`,
            },
          },
        }}
      >
        {data && (
          <Table
            columns={[
              { name: "mac", align: "left" },
              { name: "name", align: "center" },
              { name: "env. factor", align: "center" },
              { name: "location", align: "center" },
              { name: "gauge", align: "center" },
              { name: "", align: "center" },
            ]}
            rows={data.map((item) => ({
              mac: (
                <SuiBox display="flex" alignItems="center">
                  <SuiBox mr={2}>
                    <SuiAvatar src={routerIcon} alt={item.name} size="sm" variant="rounded" />
                  </SuiBox>
                  <SuiTypography variant="button" fontWeight="medium">
                    {item.macAddress}
                  </SuiTypography>
                </SuiBox>
              ),
              name: (
                <SuiTypography variant="caption" color="secondary">
                  {item.name}
                </SuiTypography>
              ),
              "env. factor": (
                <SuiTypography variant="caption" color="secondary">
                  {item.envFactor}
                </SuiTypography>
              ),
              location: (
                <SuiTypography variant="caption" color="secondary">
                  {Locations[item.location]}
                </SuiTypography>
              ),
              gauge: <Gauge data={item.gauge} />,
              "": (
                <ButtonGroup variant="text" aria-label="text button group" color="text">
                  <SuiButton variant="text" color="dark">
                    <Icon>edit</Icon>
                  </SuiButton>
                  <SuiButton variant="text" color="error">
                    <Icon>delete</Icon>
                  </SuiButton>
                </ButtonGroup>
              ),
            }))}
          />
        )}
      </SuiBox>
    </Card>
  );
}
