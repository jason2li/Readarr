import React, { Component } from 'react';
import PageContent from 'Components/Page/PageContent';
import PageContentBodyConnector from 'Components/Page/PageContentBodyConnector';
import AboutConnector from './About/AboutConnector';
import DiskSpaceConnector from './DiskSpace/DiskSpaceConnector';
import HealthConnector from './Health/HealthConnector';
import MoreInfo from './MoreInfo/MoreInfo';

class Status extends Component {

  //
  // Render

  render() {
    return (
      <PageContent title="Status">
        <PageContentBodyConnector>
          <HealthConnector />
          <DiskSpaceConnector />
          <AboutConnector />
          <MoreInfo />
        </PageContentBodyConnector>
      </PageContent>
    );
  }

}

export default Status;
