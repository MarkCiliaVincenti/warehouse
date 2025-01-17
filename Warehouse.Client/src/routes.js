/**
  All of the routes for the Soft UI Dashboard React are added here,
  You can add a new route, customize the routes and delete the routes here.

  Once you add a new route on this file it will be visible automatically on
  the Sidenav.

  For adding a new route you can follow the existing routes in the routes array.
  1. The `type` key with the `collapse` value is used for a route.
  2. The `type` key with the `title` value is used for a title inside the Sidenav. 
  3. The `type` key with the `divider` value is used for a divider between Sidenav items.
  4. The `name` key is used for the name of the route on the Sidenav.
  5. The `key` key is used for the key of the route (It will help you with the key prop inside a loop).
  6. The `icon` key is used for the icon of the route on the Sidenav, you have to add a node.
  7. The `collapse` key is used for making a collapsible item on the Sidenav that has other routes
  inside (nested routes), you need to pass the nested routes inside an array as a value for the `collapse` key.
  8. The `route` key is used to store the route location which is used for the react router.
  9. The `href` key is used to store the external links location.
  10. The `title` key is only for the item with the type of `title` and its used for the title text on the Sidenav.
  10. The `component` key is used to store the component of its route.
*/

// Soft UI Dashboard React icons
import Shop from "examples/Icons/Shop";
import Office from "examples/Icons/Office";
import Settings from "examples/Icons/Settings";
import Document from "examples/Icons/Document";
import SpaceShip from "examples/Icons/SpaceShip";
import CustomerSupport from "examples/Icons/CustomerSupport";
import CreditCard from "examples/Icons/CreditCard";
import Icon from "@mui/material/Icon";

// Soft UI Dashboard React layouts
import Dashboard from "layouts/dashboard";
import Tables from "layouts/tables";
import Billing from "layouts/billing";
import RTL from "layouts/rtl";
import Profile from "layouts/profile";
import SignIn from "layouts/authentication/sign-in";
import SignUp from "layouts/authentication/sign-up";
import WarehouseAlerts from "./layouts/alerts";
import Security from "./layouts/security";
import Products from "layouts/products";
import Users from "layouts/users";
import Warehouse from "layouts/warehouse";
import Home from "layouts/home";
import SiteConfiguration from "layouts/site-configuration";
import Beacons from "layouts/beacons";
import Providers from "./layouts/providers";

const routes = [
  {
    type: "route",
    name: "Dashboard",
    key: "dashboard",
    route: "/dashboard",
    icon: <Shop size="12px" />,
    component: <Dashboard />,
    noCollapse: true,
    protected: true,
  },
  {
    type: "collapse",
    name: "Dashboard",
    key: "home",
    route: "/home",
    icon: <Icon fontSize="12px">dashboard</Icon>,
    component: <Home />,
    noCollapse: true,
    protected: true,
  },
  {
    type: "route",
    name: "Warehouse",
    key: "warehouse",
    route: "/warehouse",
    icon: <Icon fontSize="12px">store</Icon>,
    component: <Warehouse />,
    noCollapse: true,
    protected: true,
  },
  { type: "title", title: "Management", key: "management-pages" },
  {
    type: "collapse",
    name: "Products",
    key: "products",
    route: "/products",
    icon: <Icon fontSize="12px">qr_code</Icon>,
    component: <Products />,
    noCollapse: true,
    protected: true,
  },
  {
    type: "collapse",
    name: "Sites",
    key: "configuration",
    route: "/configuration",
    icon: <Icon fontSize="12px">tab</Icon>,
    component: <SiteConfiguration />,
    noCollapse: true,
    protected: true,
  },
  {
    type: "collapse",
    name: "Beacons",
    key: "beacons",
    route: "/beacons",
    icon: <Icon fontSize="12px">sensors</Icon>,
    component: <Beacons />,
    noCollapse: true,
    protected: true,
  },
  {
    type: "collapse",
    name: "Alerts",
    key: "alerts",
    route: "/alerts",
    icon: <Icon fontSize="12px">notifications</Icon>,
    component: <WarehouseAlerts />,
    noCollapse: true,
    protected: true,
  },
  {
    type: "route",
    name: "Tables",
    key: "tables",
    route: "/tables",
    icon: <Office size="12px" />,
    component: <Tables />,
    noCollapse: true,
    protected: true,
  },
  {
    type: "route",
    name: "Billing",
    key: "billing",
    route: "/billing",
    icon: <CreditCard size="12px" />,
    component: <Billing />,
    noCollapse: true,
    protected: true,
  },
  {
    type: "route",
    name: "RTL",
    key: "rtl",
    route: "/rtl",
    icon: <Settings size="12px" />,
    component: <RTL />,
    noCollapse: true,
    protected: true,
  },
  { type: "title", title: "Administration", key: "account-pages" },
  {
    type: "route",
    name: "Profile",
    key: "profile",
    route: "/profile",
    icon: <CustomerSupport size="12px" />,
    component: <Profile />,
    noCollapse: true,
    protected: true,
  },
  {
    type: "collapse",
    name: "Providers",
    key: "providers",
    route: "/providers",
    icon: <Icon fontSize="12px">auto_awesome_motion</Icon>,
    component: <Providers />,
    noCollapse: true,
    protected: true,
  },
  {
    type: "collapse",
    name: "Users",
    key: "users",
    route: "/users",
    icon: <Icon fontSize="12px">people</Icon>,
    component: <Users />,
    noCollapse: true,
    protected: true,
  },
  {
    type: "collapse",
    name: "Security",
    key: "security",
    route: "/security",
    icon: <Icon fontSize="12px">security</Icon>,
    component: <Security />,
    noCollapse: true,
    protected: true,
  },
  {
    type: "route",
    name: "Sign In",
    key: "sign-in",
    route: "/authentication/sign-in",
    icon: <Document size="12px" />,
    component: <SignIn />,
    noCollapse: true,
  },
  {
    type: "route",
    name: "Sign Up",
    key: "sign-up",
    route: "/authentication/sign-up",
    icon: <SpaceShip size="12px" />,
    component: <SignUp />,
    noCollapse: true,
  },
];

export default routes;
