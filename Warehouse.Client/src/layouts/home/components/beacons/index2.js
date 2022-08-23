import Card from "@mui/material/Card";
import SuiBox from "components/SuiBox";
import SuiInput from "../../../../components/SuiInput";
import { FixedSizeList } from "react-window";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemIcon from "@mui/material/ListItemIcon";
import SensorsSharpIcon from "@mui/icons-material/SensorsSharp";
import ListItemText from "@mui/material/ListItemText";
import { useState, useEffect } from "react";
import { useSoftUIController } from "context";
import SuiTypography from "../../../../components/SuiTypography";
import Icon from "@mui/material/Icon";

function Beacons({ items, selectedItem, onItemSelect = () => {} }) {
  const [pattern, setPattern] = useState("");
  const onSearchBeacon = (beacon) => setPattern(beacon);
  const [controller] = useSoftUIController();
  const { direction } = controller;
  let assets =
    (items &&
      items.filter((b) => {
        return Boolean(
          !pattern || b.macAddress.toLocaleUpperCase().indexOf(pattern.toLocaleUpperCase()) > -1
        );
      })) ||
    [];

  useEffect(() => {
    assets.length > 0 && onItemSelect(assets[0]);
  }, [items]);

  const Row = ({ index, style }) => (
    <ListItem
      key={`b_${index}`}
      style={style}
      component="div"
      disablePadding
      onClick={() => onItemSelect(assets[index])}
      sx={{
        borderBottom: ({ borders: { borderWidth, borderColor } }) =>
          `${borderWidth[1]} solid ${borderColor}`,
      }}
      selected={Boolean(selectedItem) && assets[index].macAddress === selectedItem.macAddress}
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
    <Card>
      <SuiBox display="flex" justifyContent="space-between" alignItems="center" p={3}>
        <SuiBox>
          <SuiTypography variant="h6" gutterBottom>
            Beacons
          </SuiTypography>
          <SuiBox display="flex" alignItems="center" lineHeight={0}>
            <Icon
              sx={{
                fontWeight: "bold",
                color: ({ palette: { info } }) => info.main,
                mt: -0.5,
              }}
            >
              done
            </Icon>
            <SuiTypography variant="button" fontWeight="regular" color="text">
              &nbsp;<strong>{assets.length}</strong> items
            </SuiTypography>
          </SuiBox>
        </SuiBox>
      </SuiBox>
      <SuiBox px={2}>
        <SuiInput
          className="search-products"
          placeholder="Type here..."
          icon={{
            component: "search",
            direction: "left",
          }}
          onChange={(event) => onSearchBeacon(event.target.value)}
        />
      </SuiBox>
      <SuiBox
        px={2}
        py={1}
        component="ul"
        display="flex"
        flexDirection="column"
        sx={{
          height: "639px",
        }}
      >
        <FixedSizeList className="List" height={639} itemCount={assets.length} itemSize={65}>
          {Row}
        </FixedSizeList>
      </SuiBox>
    </Card>
  );
}

export default Beacons;
