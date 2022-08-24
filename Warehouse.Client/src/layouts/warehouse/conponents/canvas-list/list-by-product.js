import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
} from "../../../home/components/sites/components/accordion";
import SuiBox from "components/SuiBox";
import SuiInput from "components/SuiInput";
import { FixedSizeList } from "react-window";
import SuiTypography from "components/SuiTypography";
import * as React from "react";
import { useEffect, useState } from "react";
import { useSoftUIController } from "../../../../context";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemIcon from "@mui/material/ListItemIcon";
import QrCode2SharpIcon from "@mui/icons-material/QrCode2Sharp";
import ListItemText from "@mui/material/ListItemText";
import SensorsSharpIcon from "@mui/icons-material/SensorsSharp";

const CanvasListByProduct = ({
  selectedProduct = { products: [] },
  selectedSite,
  onSiteSelect = () => {},
  selectedBeacon = "",
  onBeaconSelect = () => {},
}) => {
  const [pattern, setPattern] = useState("");
  const [controller] = useSoftUIController();
  const [expanded, setExpanded] = React.useState("");

  const { direction } = controller;
  const handleChange = (panel, row) => (event, newExpanded) => {
    setExpanded(newExpanded ? panel : false);
    onSiteSelect(row);
  };
  const onSearch = (searchTerm) => setPattern(searchTerm);
  useEffect(() => {
    selectedProduct && setExpanded(`panel_${selectedSite.id}`);
  }, []);

  let assets =
    (selectedSite &&
      selectedSite.beacons.filter((b) => {
        return Boolean(
          !pattern || b.macAddress.toLocaleUpperCase().indexOf(pattern.toLocaleUpperCase()) > -1
        );
      })) ||
    [];

  const Row = ({ index, style }) => (
    <ListItem
      key={`b_${index}`}
      style={style}
      component="div"
      disablePadding
      onClick={() => onBeaconSelect(assets[index])}
      sx={{
        borderBottom: ({ borders: { borderWidth, borderColor } }) =>
          `${borderWidth[1]} solid ${borderColor}`,
      }}
      selected={selectedBeacon && assets[index].macAddress === selectedBeacon.macAddress}
      //secondaryAction={ }
    >
      <ListItemButton dir={direction}>
        <ListItemIcon>
          <SensorsSharpIcon fontSize="large" />
        </ListItemIcon>
        <ListItemText
          primaryTypographyProps={{ color: "#344767", fontSize: "0.875rem" }}
          primary={assets[index].name}
          secondaryTypographyProps={{ color: "#8392ab", fontSize: "0.75rem" }}
          secondary={assets[index].macAddress}
        />
      </ListItemButton>
    </ListItem>
  );
  return (
    <SuiBox pb={3}>
      {selectedProduct &&
        selectedProduct.sites.map((item, index) => (
          <Accordion
            expanded={expanded === `panel_${item.id}`}
            onChange={handleChange(`panel_${item.id}`, item)}
            key={`site_${index}`}
            TransitionProps={{ unmountOnExit: true }}
          >
            <AccordionSummary
              aria-controls={`panel_${item.id}_content`}
              id={`panel_${item.id}_header`}
              sx={{ "& .MuiAccordionSummary-content": { margin: "7px 0" } }}
            >
              <SuiBox
                display="flex"
                justifyContent="space-between"
                alignItems="center"
                style={{ width: "100%" }}
              >
                <SuiBox display="flex" alignItems="center" px={1} py={0.5}>
                  <SuiBox mr={2}>
                    <QrCode2SharpIcon fontSize="large" />
                  </SuiBox>
                  <SuiBox display="flex" flexDirection="column">
                    <SuiTypography variant="button" fontWeight="medium" color={"info"}>
                      {item.name}
                    </SuiTypography>
                    <SuiTypography variant="caption" color="secondary">
                      {item.beacons.length}&nbsp;beacons
                    </SuiTypography>
                  </SuiBox>
                </SuiBox>
              </SuiBox>
            </AccordionSummary>
            <AccordionDetails>
              <SuiBox py={1}>
                <SuiInput
                  className="search-products"
                  placeholder="Type here..."
                  icon={{
                    component: "search",
                    direction: "left",
                  }}
                  onChange={(event) => onSearch(event.target.value)}
                />
              </SuiBox>
              <SuiBox
                component="ul"
                display="flex"
                flexDirection="column"
                sx={{
                  height: "350px",
                }}
              >
                <FixedSizeList
                  className="List"
                  height={350}
                  itemCount={assets.length}
                  itemSize={65}
                >
                  {Row}
                </FixedSizeList>
              </SuiBox>
            </AccordionDetails>
          </Accordion>
        ))}
    </SuiBox>
  );
};

export default CanvasListByProduct;
